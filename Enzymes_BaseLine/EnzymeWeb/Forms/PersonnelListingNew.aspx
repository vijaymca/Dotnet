<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonnelListingNew.aspx.cs" Inherits="EnzymeWeb.PersonnelListingNew" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Personnel Listing</title>
    <base target="_self" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="700px"><tr><td align="center" width="700px">
    

    <dx:ASPxGridView ID="grdViewPersonnelListing" runat="server"
            AutoGenerateColumns="False" KeyFieldName="HSUPersons_ID"
            Theme="Office2003Blue" >
        <Columns>
		      <dx:GridViewDataCheckColumn  VisibleIndex="0" Caption="" >
            <DataItemTemplate>
			   <input id="selectRB" type="radio"  /> 
	           </DataItemTemplate> 
            </dx:GridViewDataCheckColumn>         
            <dx:GridViewDataTextColumn Name="Personname" FieldName="Person" 
                VisibleIndex="1" Caption="Person" >
               <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Bold="True" 
                    ForeColor="Black" />
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="Position" FieldName="Position" 
                VisibleIndex="2" Caption="Position" >
                <HeaderStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" 
                    HorizontalAlign="Left" VerticalAlign="Middle" />
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="EmployeeStatus" FieldName="Employment_Status" 
                VisibleIndex="3" Caption="Employment Status"  >
                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" />
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="FTE" FieldName="FTE_Type" VisibleIndex="4" 
                Caption="FTE Type" >
                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" />
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Name="WorkedHours" FieldName="Worked_Hours" 
                VisibleIndex="5" Caption="Worked Hours"  >                
                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" />
                
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Name="UserName" FieldName="UserName" 
                VisibleIndex="6"  >                
                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" />
                
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Name="Locaion" FieldName="SiteName" 
                VisibleIndex="7" Caption="Location"  >                
                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle" />
                
                <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewDataTextColumn>  
        </Columns>
		<SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="true"  
			ProcessSelectionChangedOnServer="true" AllowSelectByRowClick="True"/>
    </dx:ASPxGridView> 
      
    </td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td align="center">
    <dx:ASPxButton ID="ASPxButton1" Text="Submit" runat="server" 
            onclick="btnSubmit_Click" AutoPostBack="False" CausesValidation="False"></dx:ASPxButton>
            </td></tr>
    </table>
  
     </div>
    </form>
</body>
</html>
