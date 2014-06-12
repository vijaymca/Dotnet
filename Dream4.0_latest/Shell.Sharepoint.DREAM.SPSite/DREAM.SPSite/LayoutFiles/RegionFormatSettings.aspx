<!--
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: DisplayOption.aspx
-->

<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_CONTROLTEMPLATES/DREAM/RegionFormatSettings.ascx" TagName="RegionFormat"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Region Format Settings</title>
    <link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="/_layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel4_0.js"></script>
   <script type="text/javascript" language="javascript" src="/_layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:RegionFormat id="RegionFormat" runat="server">
            </uc1:RegionFormat>
        </div>
    </form>
</body>
</html>
