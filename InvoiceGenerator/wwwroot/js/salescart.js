var items = [];

var item = {
    productname: '',
    productid: '0',
    unitprice: '',
    quantity: '',
    totalpriceforitem: ''
};

$(document).ready(function () {

     
    //get the product
    $("#ddlproduct").on('change', function (e) {
        item.productid = e.target.value;
        item.productname = e.target.selectedOptions[0].text;

        
        //make ajax request to get unit price of the selected product
        if (item.productid != 0) {
            $.ajax({
                type: 'GET',
                url: '/Admin/SalesInvoices/GetProductByid?productid=' + item.productid,
                contentType: 'json',
                success: function (result) {

                    //set the unit price of the product
                    item.unitprice = result.price;

                    //populate textbox
                    $("#unitprice").val(item.unitprice);
                }
            });
        }
    });
 
    $("#quantity").on('input', function (e) {
        item.quantity = e.target.value;
        setprice(item);
    });

    $("#additem").on('click', function () {

        //check if the product is selected
        if (item.productid == "0") {
            alert('select product');
            return false;
        }


        //add items to array
        //check if already exist
        if (items.length > 0) {

            var productalreadyexist = items.find(x => x.productid === item.productid);
            if (productalreadyexist == undefined) {
                appenditemrow(item);

                clearitem();
            }
            else {

                $("#ddlproduct").val('0');
                alert('product is already added')
            }
        }
        else {
            appenditemrow(item);

            clearitem();
        }



       

    });


});

const setprice = (product) => {

    if (product.unitprice != NaN && product.quantity != NaN) {
        $("#totalpriceforitem").val(product.unitprice * product.quantity);
        item.totalpriceforitem = product.unitprice * product.quantity;
    }


}

const removeitem = (itemtoremove,id) => {
    $(`#productlineitems tr#${itemtoremove}`).remove();

    //remove also from array
    items = items.filter((obj) => obj.productid !== id);

    console.log(items);
}

const appenditemrow = (item) => {

    //add rows on button click
    var markup = `<tr id='tablerow_${item.productid}'>
                <td>${item.productname}</td>
                <td>${item.unitprice}</td>
                <td><input type="number" class="form-control" value='${item.quantity}' /></td>
                <td> ${item.totalpriceforitem} </td>
                <td class="text-right"><button type="button" id="removeitem" onclick="removeitem('tablerow_${item.productid}','${item.productid}')" class="btn"><i class="zmdi zmdi-delete zmdi-hc-2x " aria-hidden="true"></i></button></td>
            </tr>`;

    $("#productlineitems tbody").append(markup);

    //add to array
    items.push(item);


    //reset the product dropdown
    $("#ddlproduct").val('0');
    $("#unitprice").val("");
    $("#quantity").val("");
    $("#totalpriceforitem").val("");

}

const clearitem = () => {
    item = {
        productname: '',
        productid: '0',
        unitprice: '',
        quantity: '',
        totalpriceforitem: ''
    };
}