
$(document).ready(function () {

     
    $('#ddlpayblecustomersalesreturn').select2();
    $('#ddlsalesinvoicesalesreturn').select2();
    $('#ddlpayblecustomersalesreturn').on('select2:select', function (e) {

        //reset
        //populate the salesinvoice dropdown 
        $('#ddlsalesinvoicesalesreturn').empty();
        

        var selectedcustomer = e.params.data;

        item.customerid = selectedcustomer.id;
        item.customername = selectedcustomer.text;

        //make ajax request to get unit price of the selected product
        if (item.customerid != 0)
        {
            //set customer id
            $("#SelectedCustomerId").val(item.customerid);

            //in add screen so no selected value required in response
            populatesalesinvoicedropdown(item.customerid,0);
 
        }

    });


    $('#ddlsalesinvoicesalesreturn').on('select2:select', function (e) {

      
        var selectedinvoice = e.params.data;
        item.salesinvoiceid = selectedinvoice.id;
        item.salesinvoicetext = selectedinvoice.text;

        //make ajax request to get unit price of the selected product
        if (item.customerid != 0) {

            //set salesinvoice id
            $("#SelectedSalesInvoiceId").val(item.salesinvoiceid);


            getsalesinvoicewithlineitems(item.salesinvoiceid);

        }

    });
     
    //this means in a edit mode
    var SelectedSalesInvoiceId = $("#SelectedSalesInvoiceId").val();   
    if (SelectedSalesInvoiceId) {

        var selectedcustomerid = $("#SelectedCustomerId").val();

        //selected invoice required
        populatesalesinvoicedropdown(selectedcustomerid, SelectedSalesInvoiceId);

        //show the lineitems
        getsalesinvoicewithlineitems(SelectedSalesInvoiceId);
       

    }

    function populatesalesinvoicedropdown  (customerid, selectedinvoice)   {

        $.ajax({
            type: 'GET',
            url: '/Admin/Payment/GetSalesInvoiceByCustomersId?customerid=' + customerid + '&selectedinvoice=' + selectedinvoice,
            contentType: 'json',
            success: function (datas) {

               
                //populate the salesinvoice dropdown
                $('#ddlsalesinvoicesalesreturn').select2({
                    data: datas.results,
                }); 

            }
        });
    }


    function getsalesinvoicewithlineitems (salesinvoiceid) {
        $.ajax({
            type: 'GET',
            url: '/Admin/SalesReturn/GetGrandTotalWithLineItemsBySalesInvoiceId?salesinvoiceid=' + salesinvoiceid,
            contentType: 'json',
            success: function (response) {

                var data = JSON.parse(response);

                //clear the table
                $("#salesreturnproductlineitems tbody").empty();

                //set the grand total of the selected invoice                    
                $("#SalesReturn_RefundableAmount").val(data.GrandTotal);
                $("#SalesReturn_InvoiceCode").val(data.InvoiceCode);



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
                        LineItemId: 0
                    };

                    //generate product row items with edit and delete
                    appenditemrow(newitem);
                })
            }
        });

    }


    const appenditemrow = (item) => {

        //add rows on button click
        var markup = `<tr id='tablerow_${item.productid}'>
                <td>${item.productname}</td>
                <td>${item.unitprice}</td>
                <td>${item.quantity}</td>
                <td class='itemtotal_price'> ${item.price} </td>                
            </tr>`;

        
        $("#salesreturnproductlineitems tbody").append(markup); 
    }

})