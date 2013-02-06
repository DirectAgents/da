<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
        <script type="text/javascript">
            alert("calling");
            $(function () {
                var leadData = { id: "test"};
                $.ajax({
                    type: "POST",
                    url: "track.asmx/lead",
                    data: leadData,
                    dataType: "json",
                    success: function () {
                        alert("success");
                    }
                });
            });
    </script>
</body>
</html>
