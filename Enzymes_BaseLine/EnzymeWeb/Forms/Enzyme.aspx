<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EnzymeMain.Master"
    AutoEventWireup="true" CodeBehind="Enzyme.aspx.cs" Inherits="EnzymeWeb.Forms.Enzyme" %>

<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.2, Version=13.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div align="center">
        <dx:ASPxGridView ID="gvEnzyme" ClientInstanceName="gvEnzyme" runat="server" OnRowUpdating="OnRowUpdating"
            OnRowDeleting="OnRowDeleting" KeyFieldName="Enzyme_ID" Width="100%" AutoGenerateColumns="False"
            OnRowInserting="OnRowInserting">
            <Columns>
                <dx:GridViewCommandColumn ShowNewButtonInHeader="true" VisibleIndex="0" />
                <dx:GridViewCommandColumn Width="30px" ShowEditButton="true" VisibleIndex="1" Caption="Edit" />
                <dx:GridViewCommandColumn Width="30px" ShowDeleteButton="true" VisibleIndex="2" Caption="Delete" />
                <dx:GridViewDataTextColumn FieldName="EnzymeName" Caption="Enzyme" VisibleIndex="3">
                    <EditFormSettings ColumnSpan="2" Caption="* Title :" />
                    <PropertiesTextEdit>
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Please enter valid title" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                    <EditFormSettings VisibleIndex="1" />
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsEditing Mode="PopupEditForm">
            </SettingsEditing>
            <SettingsText ConfirmDelete="Are you sure, You want to delete?" />
        </dx:ASPxGridView>
    </div>
</asp:Content>
