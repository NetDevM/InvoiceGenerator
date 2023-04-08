var orders = {
    LineItems: [],
    customerid: '',
    paymentmethod: '',
    paymentstatus: '',
    notes: '',
    shipping: 0,
    tax: 0,
    grandtotal: 0,
    discountpercentage: 0,
    SalesInvoiceId: 0,
    invoicecode: ''
};
var items = [];
var item = {

    productid: '0',
    productname: '',
    unitprice: '',
    quantity: '',
    price: '',
    SalesInvoiceId: 0,
    SalesInvoice: null,
    LineItemId: 0
};
var grandtotal = 0;
var shippingamount = 0;
var discountamount = 0;
var isgenerateinvoicebuttondisabled = true;
var isupdateinvoicebuttondisabled = true;

$(document).ready(function () {

    //check if the salesinvoiceid is there
    //if yes its from edit else from add
    var salesinvoiceid = $("#SalesInvoiceId").val();
    if (salesinvoiceid) {


        //fetch the data
        getandpopulatesalesinvoicedatabysalesinvoiceid(salesinvoiceid);


    }

    //disable generate/update invoice button until calculate is pressed
    $("#generateinvoice").prop('disabled', isgenerateinvoicebuttondisabled);
    $("#updateinvoice").prop('disabled', isupdateinvoicebuttondisabled);


    //get the product
    $('#ddlproduct').select2();
    $('#ddlproduct').on('select2:select', function (e) {
        var selectedproduct = e.params.data;

        item.productid = selectedproduct.id;
        item.productname = selectedproduct.text;

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
 

    $('#ddlcustomers').select2();
    $('#ddlcustomers').on('select2:select', function (e) {
        var customerid = e.params.data.id;
        orders.customerid = customerid;
    });

    $("#ddlpaymentmethod").on('change', function (e) {
        orders.paymentmethod = e.target.value;
    });

    $("#ddlpaymentstatus").on('change', function (e) {
        orders.paymentstatus = e.target.value;
    });

    $("#specialNotes").on('change', function (e) {
        orders.notes = e.target.value;
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


        $("#generateinvoice").prop('disabled', isgenerateinvoicebuttondisabled);
        $("#updateinvoice").prop('disabled', isupdateinvoicebuttondisabled);

    })

    $("#generateinvoice").on('click', function () {

        //push the items to invoicedata products array
        orders.LineItems.push(items);
        createinvoicedata(orders);
    })

    $("#updateinvoice").on('click', function () {

        //push the items to invoicedata products array
        orders.LineItems.push(items);
        updateinvoicedata(orders);
    })
});
 
const setprice = (product) => {

    if (product.unitprice != NaN && product.quantity != NaN) {
        $("#totalpriceforitem").val(product.unitprice * product.quantity);
        item.price = product.unitprice * product.quantity;
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
        orders.discountpercentage = discountamount;
    }

    if (shippingamount != 0 && shippingamount != "" && shippingamount != undefined && shippingamount != NaN) {
        grandtotal += parseInt(shippingamount);
        orders.shipping = shippingamount;
    }


    $("#grandtotal").text(grandtotal.toLocaleString());
    orders.grandtotal = grandtotal;

    //enable only if there is some items
    if (orders.grandtotal > 0) {
        isgenerateinvoicebuttondisabled = false;
        isupdateinvoicebuttondisabled = false;

    }


}

const appenditemrow = (item) => {

    //add rows on button click
    var markup = `<tr id='tablerow_${item.productid}'>
                <td>${item.productname}</td>
                <td>${item.unitprice}</td>
                <td>${item.quantity}</td>
                <td class='itemtotal_price'> ${item.price} </td>
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
        price: ''
    };
}

const createinvoicedata = (order) => {

    var settings = {
        url: "/Admin/SalesInvoices/AddSales",
        method: "POST",
        timeout: 0,
        headers: {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({ "Lineitems": orders.LineItems.flat(), "Orders": orders }),
    };

    $.ajax(settings).done(function (response) {

        //display status message
        var classtoappend = 'Error'; 
        if (!response.includes('Error')) {
            classtoappend = 'success';       
        } 

        $("#notificationbar").css('display','flex');
        $("#notificationbar").addClass(`badge badge-${classtoappend}`);
        $("#notificationbar").append(response);

        $(function () {
            setTimeout(function () {
                $("#notificationbar").css("display", "none");
            }, 5000);
        });
    });

}

const updateinvoicedata = (order) => {

     var settings = {
        url: "/Admin/SalesInvoices/UpdateSalesInvoice",
        method: "POST",
        timeout: 0,
        headers: {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({ "Lineitems": orders.LineItems.flat(), "Orders": orders }),
    };

    $.ajax(settings).done(function (response) {

        //display status message
        var classtoappend = 'Error';
        if (!response.includes('Error')) {
            classtoappend = 'success';
        }
        $("#notificationbar").css('display', 'flex');
        $("#notificationbar").addClass(`badge badge-${classtoappend}`);
        $("#notificationbar").append(response);

        $(function () {
            setTimeout(function () {
                $("#notificationbar").css("display", "none");
            }, 5000);
        });

    });

}

const getandpopulatesalesinvoicedatabysalesinvoiceid = (salesinvoiceid) => {
    var settings = {
        url: `/Admin/SalesInvoices/GetSalesInvoiceWithlineItemsBySalesInvoiceId?salesinvoiceid=${salesinvoiceid}`,
        method: "GET",
        timeout: 0,
        headers: {
            "Content-Type": "application/json"
        }
    };

    $.ajax(settings).done(function (response) {

        var data = JSON.parse(response);
        console.log(data);

        //populate the data in variables
        orders.customerid = data.CustomerId;
        orders.invoicecode = data.InvoiceCode;
        orders.paymentmethod = data.PaymentMethod;
        orders.paymentstatus = data.PaymentStatus;
        orders.notes = data.Notes;
        orders.shipping = data.Shipping;
        orders.tax = data.tax;
        orders.grandtotal = data.GrandTotal;
        orders.discountpercentage = data.DiscountPercentage;
        orders.SalesInvoiceId = salesinvoiceid;

        $("#shipping").val(data.Shipping);
        $("#discount").val(data.DiscountPercentage);
        $("#specialNotes").val(data.Notes);
        discountamount = data.DiscountPercentage;
        shippingamount = data.Shipping;

        //data.SalesProductLineItems;
        data.SalesProductLineItems.map(item => {

            //for lineitemobject
            var newitem = {

                productid: item.ProductId.toString(),
                productname: item.ProductName,
                unitprice: item.UnitPrice.toString(),
                quantity: item.Quantity.toString(),
                price: item.Price.toString(),
                SalesInvoiceId: item.SalesInvoiceId.toString(),
                SalesInvoice: null,
                LineItemId: 0// item.LineItemId.toString()
            };

            //generate product row items with edit and delete
            appenditemrow(newitem);
        })

        //trigger dropdown change for customer
        $('#ddlcustomers').val(data.CustomerId);
        $('#ddlcustomers').trigger('change');

        //trigger dropdown change for paymentmethod
        $('#ddlpaymentmethod').val(data.PaymentMethod);
        $('#ddlpaymentmethod').trigger('change');

        //trigger dropdown change for paymentstatus
        $('#ddlpaymentstatus').val(data.PaymentStatus);
        $('#ddlpaymentstatus').trigger('change');


        //call compute total
        computetotal();

    });
}