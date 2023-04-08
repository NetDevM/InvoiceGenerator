
$(document).ready(function () {

     
    $('#ddlpayblecustomer').select2();
    $('#ddlsalesinvoice').select2();
    $('#ddlpayblecustomer').on('select2:select', function (e) {

        //reset
        //populate the salesinvoice dropdown 
        $('#ddlsalesinvoice').empty();
        

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


    $('#ddlsalesinvoice').on('select2:select', function (e) {

      
        var selectedinvoice = e.params.data;

        item.salesinvoiceid = selectedinvoice.id;
        item.salesinvoicetext = selectedinvoice.text;

        //make ajax request to get unit price of the selected product
        if (item.customerid != 0) {

            //set salesinvoice id
            $("#SelectedSalesInvoiceId").val(item.salesinvoiceid);

            $.ajax({
                type: 'GET',
                url: '/Admin/Payment/GetGrandTotalBySalesInvoiceId?salesinvoiceid=' + item.salesinvoiceid,
                contentType: 'json',
                success: function (data) {

                    //set the grand total of the selected invoice
                    $("#Payment_GrandTotal").val(data);
                    

                }
            });



        }

    });

    //due amount calculate
    $("#Payment_ReceivedAmount").on('change', function (e) {
        
        if (Number(e.target.value)) {

            var grandtotal = $("#Payment_GrandTotal").val();
            var amountreceived = e.target.value;

            $("#Payment_DueAmount").val(parseFloat(grandtotal - amountreceived));
        }
        else {
            $("#Payment_DueAmount").val();
        }

    });


    //this means in a edit mode
    var SelectedSalesInvoiceId = $("#SelectedSalesInvoiceId").val();    
    
    if (SelectedSalesInvoiceId) {

        var selectedcustomerid = $("#SelectedCustomerId").val();

        //selected invoice required
        populatesalesinvoicedropdown(selectedcustomerid, SelectedSalesInvoiceId);


    }

    function populatesalesinvoicedropdown  (customerid, selectedinvoice)   {

        $.ajax({
            type: 'GET',
            url: '/Admin/Payment/GetSalesInvoiceByCustomersId?customerid=' + customerid + '&selectedinvoice=' + selectedinvoice,
            contentType: 'json',
            success: function (datas) {

               
                //populate the salesinvoice dropdown 
                $('#ddlsalesinvoice').select2({
                    data: datas.results,
                });

            }
        });
    }


})