<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ConsumerWeb._Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/base/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/base/jquery-ui-1.8.22.custom.css" rel="stylesheet" type="text/css" />

    <script src="Script/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Script/jquery-ui-1.8.22.custom.js" type="text/javascript"></script>
        <script src="Script/jquery.cookies.2.2.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onFbPost() {
            if (!document.getElementById('fbAccessToken').value.length > 0) {
                alert("Facebook Access token required!");
                return false;
            } else if (!document.getElementById('txtFBRequest').value.length > 0) {
                alert("Post message required!");
                return false;
            }
        }

        function onTwitterPost() {
            if (!document.getElementById('twToken').value.length > 0) {
                alert("Twitter Access token required!");
                return false;
            } 
            else if (!document.getElementById('twTknSecret').value.length > 0) {
                alert("Twitter Access Secret required!");
                return false;
            }
            else if (!document.getElementById('txtTWRequest').value.length > 0) {
                alert("Post message required!");
                return false;
            }
        }
        
    </script>
    <script type="text/javascript">
        $(function () {
            $("#tabSocialMedia").tabs();
            var cookieName = "Tabs";
            $("#tabSocialMedia").tabs({
                selected: ($.cookies.get(cookieName) || 0),
                select: function (e, ui) {
                    $.cookies.set(cookieName, ui.index);
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">    
    <div id="banner">            
            <div class="gradient2">
	            <h1><span></span>Social Media Suite v1.0</h1>
            </div>
    </div>
    <div id="tabSocialMedia">
        <ul>
            <li><a href="#divFacebook">Facebook</a></li>
            <li><a href="#divTwitter">Twitter</a></li>
            <li><a href="#divGooglePlus">Google+</a></li>
        </ul>
        <div id="divFacebook">
            <table width="100%">
                <tr>
                   <td></td><td></td><td colspan="2"><asp:Button
                    ID="btnFb" runat="server" Text="Get Token" OnClick="fb_OnClick" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Token :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="fbAccessToken" runat="server" Width="800px" /></td><td></td>
                </tr>                
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td><asp:Button ID="btnGetUserProfile" OnClick="btnGetUserProfile_Click" runat="server"
                    Text="Get User Profile" ValidationGroup="FBInput" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnFBGetPosts"
                        OnClick="btnFBGetPost_Click" runat="server" Text="Search FB" ValidationGroup="FBInput" />&nbsp;&nbsp;&nbsp;<asp:Button
                            ID="btnPostMsg" OnClientClick="return onFbPost();" OnClick="btnPostMsg_Click"
                            runat="server" Text="POST (Current User)" ValidationGroup="FBInput" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Request :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtFBRequest" runat="server" Width="800px" /><asp:RequiredFieldValidator runat="server" ID="reqFBName" ControlToValidate="txtFBRequest"
                    ErrorMessage="Required!" ForeColor="Red" ValidationGroup="FBInput" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right" valign="top">Response :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtFBResponse" runat="server" Width="800px" Height="350px"
                        ReadOnly="true" TextMode="MultiLine" /></td><td></td>
                </tr>
            </table>
        </div>
        <div id="divTwitter">
            <table width="100%">
                <tr>
                   <td></td><td></td><td colspan="2"><asp:Button ID="btnTwitter" runat="server" Text="Get Token" OnClick="btnTwitter_OnClick" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Token :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="twToken" runat="server" Width="800px" /></td><td></td>
                </tr>  
                <tr>
                    <td></td><td align="right">Secret :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="twTknSecret" runat="server" Width="800px" /></td><td></td>
                </tr> 
                <tr>
                    <td></td><td align="right">Verifier :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="twVerifier" runat="server" Width="800px" /></td><td></td>
                </tr>               
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td><asp:Button ID="btnTwUserProfile" OnClick="btnTwUserProfile_Click" runat="server"
                    Text="Get User Profile" ValidationGroup="TWtoken" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnTWGetTweets"
                        OnClick="btnTWGetTweets_Click" runat="server" Text="Search TW" ValidationGroup="TWtoken" />&nbsp;&nbsp;&nbsp;<asp:Button
                            ID="btntwPost" OnClientClick="return onTwitterPost();" OnClick="btntwPost_Click"
                            runat="server" Text="Tweet (Current User)" ValidationGroup="TWtoken" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Request :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtTWRequest" runat="server" Width="800px" /><asp:RequiredFieldValidator runat="server" ID="reqTWName" ControlToValidate="txtTWRequest"
                    ErrorMessage="Input Required!" ForeColor="Red" ValidationGroup="TWtoken" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right" valign="top">Response :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtTWResponse" runat="server" Width="800px" Height="350px"
                        ReadOnly="true" TextMode="MultiLine" /></td><td></td>
                </tr>
            </table>
        </div>
        <div id="divGooglePlus">
        <table width="100%">
                <tr>
                   <td></td><td></td><td colspan="2"><asp:Button
                    ID="btnGPAccessToken" runat="server" Text="Get Token" OnClick="GP_OnClick" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Token :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="gpToken" runat="server" Width="800px" /></td><td></td>
                </tr>  
                <tr>
                    <td></td><td align="right">Secret :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="gpTknSecret" runat="server" Width="800px" /></td><td></td>
                </tr> 
                <tr>
                    <td></td><td align="right">Verifier :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="gpVerifier" runat="server" Width="800px" /></td><td></td>
                </tr>               
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td><asp:Button ID="btnGPUserProfile" OnClick="btnGPUserProfile_Click" runat="server"
                    Text="Get User Profile" ValidationGroup="GPtoken" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td></td><td></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right">Request :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtGPRequest" runat="server" Width="800px" /><asp:RequiredFieldValidator runat="server" ID="reqGPName" ControlToValidate="txtGPRequest"
                    ErrorMessage="Input Required!" ForeColor="Red" ValidationGroup="GPtoken" /></td><td></td>
                </tr>
                <tr>
                    <td></td><td align="right" valign="top">Response :&nbsp;&nbsp;&nbsp;</td><td><asp:TextBox ID="txtGPResponse" runat="server" Width="800px" Height="350px"
                        ReadOnly="true" TextMode="MultiLine" /></td><td></td>
                </tr>
            </table>            
        </div>
    </div>    
    <div id="footer">
    
    </div>
    </form>
</body>
</html>
