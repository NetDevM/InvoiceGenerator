var invoicedata = {
    products: [],
    customerid: '',
    paymentmethod: '',
    paymentstatus: '',
    notes: '',
    shipping: '',
    tax: 0,
    grandtotal: 0,
    discountpercentage: 0
};
var items = [];
var item = {
    productname: '',
    productid: '0',
    unitprice: '',
    quantity: '',
    totalpriceforitem: ''
};
var grandtotal = 0;
var shippingamount = 0;
var discountamount = 0;


$(document).ready(function () {

    //hide generate invoice button until calculate is pressed
    $("#generateinvoice").prop('disabled', true);

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

    $("#ddlcustomers").on('change', function (e) {
        invoicedata.customerid = e.target.value;
    });

    $("#ddlpaymentmethod").on('change', function (e) {
        invoicedata.paymentmethod = e.target.value;
    });

    $("#ddlpaymentstatus").on('change', function (e) {
        invoicedata.paymentstatus = e.target.value;
    });

    $("#specialNotes").on('change', function (e) {
        invoicedata.notes = e.target.value;
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

        if (item.quantity == "" || item.quantity == "0") {
            alert('enter quantity');
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

    $("#shipping").on('keyup', function (e) {
        shippingamount = e.target.value;
    });

    $("#discount").on('keyup', function (e) {
        discountamount = e.target.value;
    })

    $("#calculatetotal").on('click', function () {
        computetotal();
        $("#generateinvoice").prop('disabled', false);

    })

    $("#generateinvoice").on('click', function () {

        //push the items to invoicedata products array
        invoicedata.products.push(items);


        createinvoicedata();
    })

});



const setprice = (product) => {

    if (product.unitprice != NaN && product.quantity != NaN) {
        $("#totalpriceforitem").val(product.unitprice * product.quantity);
        item.totalpriceforitem = product.unitprice * product.quantity;
    }


}

const removeitem = (itemtoremove, id) => {
    $(`#productlineitems tr#${itemtoremove}`).remove();

    //remove also from array
    items = items.filter((obj) => obj.productid !== id);

    //reset shipping discount grandtotal
    shipping = 0;
    discount = 0;


}

const computetotal = () => {
    grandtotal = 0;
    $("#productlineitems tbody tr td.itemtotal_price").each(
        function () {
            grandtotal += eval($(this).text());
        }
    );

    if (discountamount != 0 && discountamount != "" && discountamount != undefined && discountamount != NaN) {
        grandtotal -= parseInt((discountamount / 100) * grandtotal);
        invoicedata.discountpercentage = discountamount;
    }

    if (shippingamount != 0 && shippingamount != "" && shippingamount != undefined && shippingamount != NaN) {
        grandtotal += parseInt(shippingamount);
        invoicedata.shipping = shippingamount;
    }


    $("#grandtotal").text(grandtotal.toLocaleString());
    invoicedata.grandtotal = grandtotal;


}

const appenditemrow = (item) => {

    //add rows on button click
    var markup = `<tr id='tablerow_${item.productid}'>
                <td>${item.productname}</td>
                <td>${item.unitprice}</td>
                <td>${item.quantity}</td>
                <td class='itemtotal_price'> ${item.totalpriceforitem} </td>
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

const createinvoicedata = () => {

    console.log(invoicedata);
}