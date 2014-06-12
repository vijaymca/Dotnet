/* 
 * *******************************************************************************
 * File Name        :   AssetTree.js
 * Project Name     :   SHELL_DREAM
 * Type             :   JavaScript Function  
 * Date_Created     :   Aug 14 2010
 * Version          :   1.0 
 * *******************************************************************************
 */
/***************************************************************
            onPageLinkClick event handler
***************************************************************/
function onPageLinkClick(sender)
{
    if(sender.id == "currentLink")
    {
        window.event.cancelBubble = true;
        return true;
    }
    var hdfldLinkNumber = document.getElementById(GetObjectID("hidClickedPage", "input"));
    var hdfldSearchText = document.getElementById(GetObjectID("hidSearchText", "input"));
    if((sender.innerText == "Next") || (sender.innerText == "Previous"))
    {
        hdfldLinkNumber.value = GetCurrentPageLink(sender.parentElement,sender.innerText);
    }
    else
    {
        hdfldLinkNumber.value = sender.innerText;
    }
}
/***************************************************************
           function to Get Current Page Link on click of next or previous
***************************************************************/
function GetCurrentPageLink(objSpan,linkName)
{
 var arrSpan = objSpan.getElementsByTagName("span");
 var strLink = "";
 for(var i = 0; i < arrSpan.length; i++)
 {
    if(arrSpan[i].id == "currentLink")
    {
        if(linkName == "Next")
        {
            strLink = arrSpan[i].nextSibling.nextSibling.innerText;
        }
        else if(linkName == "Previous")
        {
            strLink = arrSpan[i].previousSibling.previousSibling.innerText;
        }
        else
        {
          strLink = arrSpan[i].innerText ;
        }
        break;
    }
 }
 return strLink;
}
/***************************************************************
             function to Show Hide SearchBox
***************************************************************/
function ShowHideSearchBox(sender)
{
   var spanCollection = sender.parentNode.getElementsByTagName('SPAN')
   if((spanCollection.length>0)&&(spanCollection[0].className == 'hide'))
   {
        spanCollection = $("span:[class=show]");
        ShowHideSpan(spanCollection ,'hide')
        spanCollection = sender.parentNode.getElementsByTagName('SPAN');
        ShowHideSpan(spanCollection ,'show')
   }
   else
   {
        ShowHideSpan(spanCollection ,'hide')
   }
   //updating searchbox status
    var hidSearchBoxStatus = document.getElementById(GetObjectID("hidSearchBoxStatus", "input"));
    var textBxCollection = sender.parentNode.getElementsByTagName('INPUT');
    hidSearchBoxStatus.value = spanCollection[0].id + "|" + spanCollection[0].className + "|" + textBxCollection[0].value;
}
/***************************************************************
             function to Show SearchBox OnLoad
***************************************************************/
function ShowSearchBoxOnLoad()
{
    var hidSearchBoxStatus = document.getElementById(GetObjectID("hidSearchBoxStatus", "input"));
    if(hidSearchBoxStatus == null || hidSearchBoxStatus.value == "")
        return;
    var arrSearchBoxValues = hidSearchBoxStatus.value.split("|");
    if(arrSearchBoxValues == null || arrSearchBoxValues.length <=0)
        return;
    var spanCollection = $("span:[id="+ arrSearchBoxValues[0] + "]");//arrSearchBoxValues[0] = contains id of span
    if(spanCollection.length <=0)
        return;
    ShowHideSpan(spanCollection ,arrSearchBoxValues[1]);//arrSearchBoxValues[1] = contains id of span class name either show or hide
    var hdfldSearchText = document.getElementById(GetObjectID("hidSearchText", "input"));
    var textBxCollection = spanCollection[0].getElementsByTagName('INPUT');
    if(hdfldSearchText.value == arrSearchBoxValues[2])//arrSearchBoxValues[2] = contains value of search textbox
    {
        textBxCollection[0].value = hdfldSearchText.value;
    }
}
/***************************************************************
             function to Show Hide Span
***************************************************************/
function ShowHideSpan(spanCollection ,className)
{
 for(var i = 0; i < spanCollection.length; i++)
   {
     spanCollection[i].className = className;
   }
}
/***************************************************************
             function called onSearchClick
***************************************************************/
function onSearchClick(sender)
{
    var hdfldLinkNumber = document.getElementById(GetObjectID("hidClickedPage", "input"));
    var hdfldSearchText = document.getElementById(GetObjectID("hidSearchText", "input"));
    var hidSearchBoxStatus = document.getElementById(GetObjectID("hidSearchBoxStatus", "input"));
    var textBxCollection = sender.parentNode.getElementsByTagName('INPUT');
    
    if((textBxCollection>0)&&(textBxCollection[0].value==""))
    {
        alert("Enter some search criteria.");
        window.event.cancelBubble = true;
        return true;
    }
    hdfldLinkNumber.value = "1";
    hdfldSearchText.value = textBxCollection[0].value;
      //updating searchbox status
    hidSearchBoxStatus.value = sender.parentNode.id + "|" + sender.parentNode.className + "|" + textBxCollection[0].value;
}
/***************************************************************
             function called on ContextMenu ItemClicked
***************************************************************/
function onContextMenuItemClicked(sender, eventArgs)
{

   // var windowHeight = (window.screen.height*(3/5));
    //var windowWidth = window.screen.width;
    var windowWidth = 800;
    var windowHeight = 600;
    var tree = null;
    var arrCheckedNodes = null;
    var node = eventArgs.get_node();   
    var item = eventArgs.get_menuItem();
    if(item.get_value() == "Header")
    {
        return true;
    }
    tree = node.get_treeView();
    if(tree.get_checkedNodes().length<=0)
    {
      alert("Select atleast one asset.");
      return true;
    }  
    arrCheckedNodes = tree.get_checkedNodes();
    if(arrCheckedNodes[0].get_level() != node.get_level())
    {
        alert("Select asset of this type.");
        return true;
    }
    var hidSelectedAssets = GetHTMLObject(window,"hidSelectedRows", "input");  
    var hidSelectedCriteriaName = GetHTMLObject(window,"hidSelectedCriteriaName", "input");   
    var hidSelectedAssetNames = GetHTMLObject(window,'hidSelectedAssetNames','input');  
    var hidSearchType = GetHTMLObject(window,"hidSearchType", "input");  
    var hidAssetName = GetHTMLObject(window,"hidAssetName", "input");  
    hidSelectedAssets.value = "";
    hidSelectedAssetNames.value = "";
    var strLinkValue = item.get_attributes().getAttribute("linkValue");
    var strAsset = item.get_attributes().getAttribute("asset");
    for(var i=0;i<arrCheckedNodes.length;i++)
    {
        if((strLinkValue == "EPCatalog") || (strLinkValue == "EPCatalogWithoutFilter")|| (strLinkValue == "PVTReport"))
        {
            hidSelectedAssets.value += arrCheckedNodes[i].get_text() + "|";
        }
        else
        {
            hidSelectedAssets.value += arrCheckedNodes[i].get_value() + "|";
            hidSelectedAssetNames.value += arrCheckedNodes[i].get_text() + "|";
        }
    }
    hidSelectedCriteriaName.value = node.get_attributes().getAttribute("selectedcriterianame");
    hidSearchType.value = node.get_attributes().getAttribute("searchtype");
    hidAssetName.value = strAsset;
    var msgWindow;
    if(item.get_text().replace(/ /g,'') == "ListofWells" || item.get_text().replace(/ /g,'') == "ListofWellbores")
    {
        //example getting asset 
        //"ListofWells".substring("ListofWells".indexOf("of")+2,"ListofWells".length-1)
        strAsset = item.get_text().replace(/ /g,'');
        strAsset = strAsset.substring(strAsset.indexOf("of")+2,strAsset.length-1);
        //Dream 4.0 changes start
        window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),strAsset);  
        //Dream 4.0 changes end
        document.forms[0].action= item.get_value() + '?asset=' +strAsset+'&listSearchType=' +item.get_text().replace(/ /g,'')+'&country=0';
        document.forms[0].method = "post";
        document.forms[0].submit();
        return false;
    }
    else if(strLinkValue == "PVTReport")
    {
        msgWindow = window.open('', strLinkValue, 'width='+ windowWidth +',height=' + windowHeight + ',scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
        document.forms[0].action = item.get_value() + "?SearchType=" + encodeURIComponent(item.get_text())  + '&assetType=' + strAsset; 
    }
    else if((strLinkValue == "EPCatalog") || (strLinkValue == "EPCatalogWithoutFilter") || (strLinkValue == "DWB"))
    {
        msgWindow = window.open('', strLinkValue, 'width='+ windowWidth +',height=' + windowHeight + ',scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
        document.forms[0].action = item.get_value() + "?SearchType=" + strLinkValue  + '&assetType=' + strAsset; 
    }
//   else if((strLinkValue == "DailyWellsReporting") || (strLinkValue == "MechanicalData") || (strLinkValue == "WellHistory")|| (strLinkValue == "WellTestReport"))//report for which only single row selection is allowed
//    {
////        if(tree.get_checkedNodes().length > 1)
////        {
////          alert("Select only one asset.");
////          return true;
////        }  
//        msgWindow = window.open('', strLinkValue, 'width='+ windowWidth +',height=' + windowHeight + ',scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
//        document.forms[0].action = item.get_value() + "?SearchType=" + item.get_text().replace(/ /g,'');
//    }
    else
    {
        msgWindow = window.open('', strLinkValue, 'width='+ windowWidth +',height=' + windowHeight + ',scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
        document.forms[0].action = item.get_value() + "?SearchType=" + item.get_text().replace(/ /g,'');
    }
    document.forms[0].method = "post";
    document.forms[0].target =  strLinkValue;
    document.forms[0].submit();
    document.forms[0].target="_self";
    document.forms[0].action= msgWindow.opener.location.href;
    document.forms[0].method="post"; 
    msgWindow.focus();
}
/***************************************************************
             function to Register Page Request ManagerEvents
***************************************************************/
function RegisterPageRequestManagerEvents() 
{
    try
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
function BeginRequestHandler(sender, args)
{
    try
    {
        var tblMain = document.getElementById("tblMain")
        if(tblMain !=null)
        tblMain.className = "tableDisable";
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             EndRequestHandler
***************************************************************/
function EndRequestHandler(sender, args)
{
    try
    {
        document.getElementById("tblMain").className = "tableEnable";
        ShowSearchBoxOnLoad();
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             function called on  Client Node Checking
***************************************************************/
function ClientNodeChecking(sender, eventArgs)
{
    try
    {
        var node = eventArgs.get_node();  
        var tree = node.get_treeView();
        if(tree.get_checkedNodes().length<=0)
        return true;
        var arrCheckedNodes = tree.get_checkedNodes();
        if(arrCheckedNodes[0].get_level() == node.get_level())
        {
            return true;
        }
        if(window.confirm("You can select one asset type at a time.If you want to change asset type click OK else Cancel."))
        {
           for(var intNodeCounter=0;intNodeCounter<arrCheckedNodes.length;intNodeCounter++)
           {
                arrCheckedNodes[intNodeCounter].set_checked(false);
           }
            return true;
        }
        else
        {
           eventArgs.set_cancel(true);
           return false;
        }
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             function to Allow Event Bubbling
***************************************************************/
function AllowEventBubbling()
{
    if(arguments.length==0)
    {
        window.event.cancelBubble = true;
        return true;
    }
    var args = Array.prototype.slice.call(arguments);//converting arguments into array object
    var strIds = args.join('|'); //converting array of ids to string seperated by pipe symbol
    var strId = "";
    if(event.srcElement.name)//if name attribute is defined
    {
        strId = event.srcElement.name;
    }
    else
    {
        strId = event.srcElement.id;
    }
    if((strId == "") ||(strIds.indexOf(strId)<0))
         window.event.cancelBubble = true;
}
/***************************************************************
             function called  on Node Clicking
***************************************************************/
function onNodeClicking(sender, eventArgs)
{
    var node = eventArgs.get_node();
    var hdfldSearchText = document.getElementById(GetObjectID("hidSearchText", "input"));
    var hidRecordCount = document.getElementById(GetObjectID("hidRecordCount", "input"));
    if(node.get_value() == "treeviewpaging")
    {     
        hdfldSearchText.value = GetNodeAttribute(node,"searchtext");
        hidRecordCount.value = GetNodeAttribute(node,"recordcount");
    }
    if(node.get_value() == "error")
    {
        eventArgs.set_cancel(true);
    }
    else if(node.get_attributes().getAttribute("leafnode"))
    {
         eventArgs.set_cancel(true);
    }
}
/***************************************************************
             function to Get Node Attribute
***************************************************************/
function GetNodeAttribute(node,attributeName)
{
   var strSearchText = "";
 
   if(node.get_parent().set_checked == null)
   {
     if((strSearchText = node.get_parent().get_nodes().getNode(0).get_attributes().getAttribute(attributeName)))
     {
     }
     else
     {
      strSearchText = "";
     }
   }
   else if((strSearchText = node.get_parent().get_attributes().getAttribute(attributeName)))
   {
   }
   else
   {
   strSearchText = ""; 
   }
   return strSearchText;
}