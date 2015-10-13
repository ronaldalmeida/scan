<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scanbook.aspx.cs" Inherits="scan.Scanbook" %>

<!DOCTYPE html>

<html>
<head> 
    <script src="jquery-1.11.3.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('#btnClear').click(function ()
            {
                $('#BtnReadStatus').attr('value', 'Unread');
                $('#ISBN').val("");
                $('#comment').val(" ");
                $('#table').attr('style', 'display: none');
            }
            );
            $('#BtnReadStatus').click(function () {
                var ISBN = $('#ISBN').val();
                $.ajax({
                    url: 'GetBook.asmx/ReadStatus',
                    data: { ISBN: ISBN},
                    method: 'post',
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        $('#BtnReadStatus').attr('value', 'Read');
                                            },
                    error: function (err)
                    { alert("Insert a Valid ISBN"); }
                });
            });

            $('#BtnComment').click(function () {
                var ISBN = $('#ISBN').val();
                var comm =  $('#comment').val();
                $.ajax({
                    url: 'GetBook.asmx/Comment',
                    data: { ISBN: ISBN, comm: comm },
                    method: 'post',
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        alert("comment posted!");
                    },
                    error: function (err)
                    { alert("Insert a Valid ISBN"); }
                });
            });


            $('#btnGetBookDetails').click(function ()
            {
                var ISBN = $('#ISBN').val();
                $('#comment').val(" ") ;
                $.ajax({
                    url: 'GetBook.asmx/GetBookDetails',
                    data: { ISBN: ISBN },
                    method: 'post',
                    dataType: 'json',
                    async: true,
                    success: function (data)
                    {   
                        if (data.totalitems == 0)
                        { alert("Book Not Found, Make sure you have typed the correct ISBN"); }
                        else {

                            if (!data.source)
                            {
                                
                                $('#Title').val(data.items[0].volumeInfo.title);
                                if (data.items[0].volumeInfo.authors == null)
                                    { $('#Author').val("N/A"); }
                                else
                                    { $('#Author').val(data.items[0].volumeInfo.authors); }
                                $('#PageCount').val(data.items[0].volumeInfo.pageCount);
                                $('#table').attr('style', 'display: block');
                                $('#BtnReadStatus').attr('value', 'Unread');
                               
                            }
                            else
                            {
                                var RD = data.ReadStatus;
                                $('#table').attr('style', 'display: block');
                                $('#BtnReadStatus').attr('value', RD);
                                
                                $('#comment').val(data.comment);
                                $('#Title').val(data.title1);
                                $('#Author').val(data.authors1); 
                                $('#PageCount').val(data.pageCount1);
                            }
                        }

                    },
                    error: function (err)
                    { alert("Insert a Valid ISBN"); }
                });
            });
        });

    </script>
</head>

<body > 
    <div  style="width:auto;align-self:auto">
        <br /><br />
    ISBN : 
    <input id="ISBN" title="type" style="width:120px"/> 
    <input type="button" id="btnGetBookDetails" value="Get Book Details" />
    <input type="button" id="btnClear" value="Clear" /> <br /><br /> 


    <table id="table" style="display:none" > 
        <tr> 
        <td>Page Count&nbsp&nbsp</td> 
        <td><input id="PageCount" type="text"  style="width:600px; border:none"/></td> 
        </tr>
        <tr>
        <td>Title</td> 
        <td><input id="Title" type="text" style="width:600px; border:none" />
        </td>
        </tr> 
        <tr> 
        <td>Author</td> 
        <td><input id="Author" type="text" style="width:600px; border:none" /> </td>
        </tr>
        <tr>
        <td> <input type="button" id="BtnReadStatus" value="unred" /><br />
            
        </td>
        <td><textarea id="comment" cols="20" rows="2"></textarea> <br />
            <input type="button" id="BtnComment" value="Submit Comment" />
        </td>
           
        </tr>
    </table>
        </div>
</body>
</html>
