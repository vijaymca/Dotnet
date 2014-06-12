/* 
 * *******************************************************************************
 * File Name        :   DreamAjaxService.js
 * Project Name     :   SHELL_DREAM
 * Type             :   JavaScript Function  
 * Date_Created     :   Mar 14 2011
 * Version          :   1.0 
 * *******************************************************************************
 */

var dreamAjaxServiceProxy;
/**************************************************
 Initializes global and proxy default variables.
***************************************************/
function pageLoad()
{
    //Instantiate the service proxy.
   if((Shell == null)|| (Shell == 'undefined'))
        return;
   dreamAjaxServiceProxy = new Shell.SharePoint.DREAM.Site.UI.DateTimeConvertorService();
   if(dreamAjaxServiceProxy == null)
        return;
    //Set the default call back functions.
   dreamAjaxServiceProxy.set_defaultSucceededCallback(RequestSucceededCallback);
   dreamAjaxServiceProxy.set_defaultFailedCallback(RequestFailedCallback);
}
/**************************************************
 Function to call format dateservice
***************************************************/
function CallFormatDateService(date,objTextField)
{
    var datetime = dreamAjaxServiceProxy.GetDateTime(date,SetDateInReginalFormat,RequestFailedCallback,objTextField);
}
/**************************************************
  Function to call parse dateservice.
***************************************************/
function CallParseDateService(objTextField)
{ 
    var parsedDate = dreamAjaxServiceProxy.ParseDateTime(objTextField.value,ParseDate,RequestFailedCallback,objTextField);   
}
/**************************************************
 Function processes the service return value.
***************************************************/
function SetDateInReginalFormat(result,objTextField, methodName)
{
   objTextField.value = result;
}
/**************************************************
 Function pprocesses the service return value.
***************************************************/
function ParseDate(result,objTextField,methodName)
{
    calendarControl.show(objTextField,result);
}
/**************************************************
 Callback function that processes the service return value.
 This method is reserved for future use to show request succeded message.
**************************************************/
function RequestSucceededCallback(result)
{
   //alert(result);
}
/**************************************************
 Callback function invoked when a call to the  service methods fails.
**************************************************/
function RequestFailedCallback(error, userContext, methodName) 
{
    if(error !== null) 
    {
          alert(error.get_message());
    }
}
if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
