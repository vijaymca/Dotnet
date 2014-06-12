/* 
 * *******************************************************************************
 * File Name        :   SRPJavaScriptFunctionsRel3_0.js
 * Project Name     :   DREAM 3.0
 * Type             :   JavaScript Function  
 * Date_Created     :   Sept 6th 2010
 * Version          :   1.0 
 * *******************************************************************************
 */
 
/**************************************************
function to open SRP Field Adv Search Page
***************************************************/
/// <summary>
/// Populate TechnoSettings   DropDown Based on the TechnoClarification radio List .
/// </summary>
/// <param name="radiolistcontrol">The TechnoClarification radio List.</param>
/// <param name="ballydropId">The TechnoSettings DropDown box.</param>
 /// <param name="kleemdropId">TheTechnoSettings DropDown box.</param>
   function TechnoSettingsOnChangeVisible(radiolistcontrol,ballydropId,kleemdropId)
	{
	 var dropBallyTechnoSettings = document.getElementById(ballydropId); 
	 var dropKleemTechnoSettings = document.getElementById(kleemdropId); 
	 if(radiolistcontrol.value=="Bally")
	 {
	   dropKleemTechnoSettings.style.display = 'none';
	   dropBallyTechnoSettings.style.display = 'block';
	 }
	 else
	 {
	  dropKleemTechnoSettings.style.display = 'block';
	   dropBallyTechnoSettings.style.display = 'none';
	 }
	}
	
	
/// <summary>
/// Numeric validation .
/// </summary>
/// <param name="value">The User Entered Value.</param>
/// <param name="nodecimals">The nodecimals for Validations.</param>
 /// <param name="flag">The is Range Applicable.</param>        
function NumericMatchClick(value,nodecimals,flag)
{
    var nDExp="^-{0,1}\\d+\\.\\d{0,"+nodecimals+"}$";
    var nExp="^-{0,1}\\d+$";
    var regExp;
    if(flag)
    {
    if(value.indexOf(".")!=-1)
    {
    regExp= new RegExp(nDExp);
    }
    else
    {
      regExp= new RegExp(nExp);
    }

    }
    else
    {
    regExp= new RegExp(nExp);

    }
    if(regExp.test(value))
    {
     if(value.indexOf("-") != -1)
     {
     return false;
     }
     else if(value.indexOf("+") != -1)
     {
     return false;
     }
     return true;
    }
    else
    {                
    return false;

    } 

}

/// <summary>
///CheckNumberRange .
/// </summary>
/// <param name="value">The User Entered Value.</param>
/// <param name="Minimum">The Minimum Number.</param>
 /// <param name="Maximum">The Maximum Number.</param>        
function CheckNumberRange(value,Minimum,Maximum)
{

  if(Minimum != "infinite" && Maximum != "infinite")
  {  
  if(parseFloat(value)>=parseFloat(Minimum) && parseFloat(value)<=parseFloat(Maximum))
   {
     return true;
   }
   else
   {
     return false;
   }
  }
  if(Minimum == "infinite")
  {   
  if(parseFloat(value)<=parseFloat(Maximum))
   {
     return true;
   }
   else
   {
     return false;
   }
  }
  if(Maximum == "infinite")
  {  
  if(parseFloat(value)>=parseFloat(Minimum))
   {
     return true;
   }
   else
   {
     return false;
   }
  }
}

/// <summary>
///Checks invalid Charaters Validation in Rad Combo Boxes .
/// </summary>
/// <param name="sender">The Rad Combo Box.</param>
/// <param name="eventArgs">The eventArgs.</param>               
function HandleRequestStart(sender, eventArgs)
{ 
  
  if (eventArgs.get_text().length < 1)
   {
      eventArgs.set_cancel(true); 
   }
   else
   {
        if (!isSplCharacter(eventArgs.get_text()))
        {   sender.clearSelection(); 
            sender._focused=false;
           alert("Invalid Character.");
           eventArgs.set_cancel(true);     
        }
   }
}

/// <summary>
///  Checks for min 3 char entry before trigger server side ItemRequested event for BasinName radCombo.
///  Validates for avoidance of special characters in input.
/// </summary>
/// <param name="sender">The Rad Combo Box.</param>
/// <param name="eventArgs">The eventArgs.</param>               
function HandleBasinRequestStart(sender, eventArgs)
{ 
  if (eventArgs.get_text().length < 1)
   {
      eventArgs.set_cancel(true); 
   }
  else  if (eventArgs.get_text() == "*" || eventArgs.get_text() == "%")
    {
      alert("Wildcard characters * or % is not allowed as criteria.");
      eventArgs.set_cancel(true); 
    }  
   else
   {
        if (!isSplCharacter(eventArgs.get_text()))
        {   sender.clearSelection(); 
            sender._focused=false;
           alert("Invalid Character.");
           eventArgs.set_cancel(true);     
        }
   }
}

/// <summary>
///  Checks for min 3 char entry before trigger server side ItemRequested event for FieldName radCombo.
///  Validates for avoidance of special characters in input.
/// </summary>
/// <param name="sender">The Rad Combo Box.</param>
/// <param name="eventArgs">The eventArgs.</param>               
function HandleFieldRequestStart(sender, eventArgs)
{ 
if(eventArgs.get_text().length == "")
{
 sender.clearItems(); 
 sender.commitChanges();
}
else if (eventArgs.get_text().length < 3)
{
    if (eventArgs.get_text() == "*" || eventArgs.get_text() == "%")
    {
      alert("Wildcard characters * or % is not allowed as criteria.");
    }
  eventArgs.set_cancel(true); 
}
else
{
   sender.clearItems(); 
   sender.get_moreResultsBoxMessageElement().innerText = "";  
    if (!isSplCharacter(eventArgs.get_text()))
    {   sender.clearSelection(); 
        sender._focused=false;
       alert("Invalid Character.");
       eventArgs.set_cancel(true);     
    }
    else if (eventArgs.get_text() == "*" || eventArgs.get_text() == "%")
    {
      alert("Wildcard characters * or % is not allowed as criteria.");
    }
    else if (eventArgs.get_text().indexOf("*") != -1)
    {       
      if((eventArgs.get_text().indexOf("*") != eventArgs.get_text().length - 1) && eventArgs.get_text().indexOf("%") != (eventArgs.get_text().length - 1))
      {
        alert("Wildcard characters * or % is not allowed as criteria.");
      }
    }
    /// indexOf
 }
}

/// <summary>
///OpenFieldSelection Popup .
/// </summary>
/// <param name="radcombo">The ID Of Rad Combo Box.</param>
/// <param name="type">Type of Control </param>                
function OpenSRPPopup(radcombo,type)
{ 

/***** Reserved for future enhancement purpose *********/
/*if(type == 'Basin')
{
   /// Locate the button next to Basin List and calculate left and top position 
}
else if(type == 'Operator')
{
   /// Locate the button next to Operator List and calculate left and top position 
}*/
  window.open('/Pages/FieldAdvPop.aspx?ControlId='+radcombo+'&'+type+'=true','',"height=200px,width=300px,resizable=no,left="+ screen.width/4 + ",top=" + screen.height/3);
}

/// <summary>
///Add new Item to Rad Combo BOx .
/// </summary>
/// <param name="combo">The  Rad Combo Box.</param>
/// <param name="value">The Value </param>              
function AddNewItem(combo,value)
{
  
    if(value.indexOf(";")!=-1)
    {
     var temp=value.split(";");  //get the client-side combobox instance
     var comboBox = $find(combo);     
     var comboItem = new Telerik.Web.UI.RadComboBoxItem();
     comboItem.set_text(temp[0]);
     comboItem.set_value(temp[1]);
     //the method below indicates that client-changes are to be made
     comboBox.trackChanges();
     //add the newly created item to the Items collection of the combobox
     comboBox.get_items().add(comboItem); 
     //select the newly added item
     comboItem.select();
     //the methods below submits the client changes to the server
     //these changes are persisted after postback
     comboBox.commitChanges();
     }
}

/// <summary>
///Hide or Display SRP Control Section .
/// </summary>
/// <param name="hidSrpControlSectionId">The  Hidden Control Id.</param>                      
function HideDisplaySRPControlSection(hidSrpControlSectionId)
{

     var hidControl=document.getElementById(hidSrpControlSectionId);
     if(hidControl!=null)
        {    var hidValue=hidControl.value;
             var boolRangeNew=hidValue=="yes"?true:false;
             var tblControl=document.getElementById("tblSrpControlSection");            
             var trLogicalOperator = document.getElementById("trLogicalOperator");
             var lblSRPInfo = document.getElementById(GetObjectID('lblSRPInfo','span')); //document.getElementById("lblSRPInfo");
             var lblSRPPorosityRangeInfo = document.getElementById("lblSRPPorosityInfo");
             var lblSRPPermeabilityRangeInfo = document.getElementById("lblSRPPermeabilityInfo");
             if(tblControl!=null)
             {
                 if(boolRangeNew)
                  {
                  tblControl.style.display = 'block';                 
                  }
                  else
                  {tblControl.style.display = 'none';
                   
                  }
             }
             /// trAdvFieldSearchPetrophysicalSearch0
             for(i =0; i <= 2; i++)
            {
                 var rowControl=document.getElementById("trAdvFieldSearchPetrophysicalSearch" + i); 
                 if(rowControl != null)
                 {
                 if(boolRangeNew)
                  {
                  rowControl.style.display = 'block';                 
                  }
                  else
                  {rowControl.style.display = 'none';
                   
                  }
                 }
             }
             /// trAdvFieldSearchProductionSearch5
             for(i =0; i <= 5; i++)
            {
                 var rowControl=document.getElementById("trAdvFieldSearchProductionSearch" + i); 
                 if(rowControl != null)
                 {
                 if(boolRangeNew)
                  {
                  rowControl.style.display = 'block';                 
                  }
                  else
                  {rowControl.style.display = 'none';
                   
                  }
                 }
             }
             /// trAdvFieldSearchGeology2
              for(i =0; i <= 2; i++)
            {
                 var rowControl=document.getElementById("trAdvFieldSearchGeology" + i); 
                 if(rowControl != null)
                 {
                 if(boolRangeNew)
                  {
                  rowControl.style.display = 'block';                 
                  }
                  else
                  {rowControl.style.display = 'none';
                   
                  }
                 }
             }
               if(trLogicalOperator!=null)
             {
                 if(boolRangeNew)
                  {
               
                  trLogicalOperator.style.display = 'block';
                  }
                  else
                  {
                   trLogicalOperator.style.display = 'none';
                  }
             }
             if(lblSRPInfo != null)
             {
                if(boolRangeNew)
                {
                lblSRPInfo.style.display = 'block';
                }
                else
                {
                 lblSRPInfo.style.display = 'none';
                }
             }
        }          

}

/// <summary>
///NumericValidations for Text Boxes .
/// </summary>
/// <param name="txtcontrolids">The  Text Boxes  Ids.</param>       
function NumericValidation(txtcontrolids)
{
     var temp=txtcontrolids.split(";");
     var i=0;
    for(i=0; i<temp.length;i++)
    { 
        if(temp[i]!=null && temp[i].length>0)
        {
            var txtControl=document.getElementById(temp[i]);
            if(txtControl!=null)
            {       
          var Controlvalue= Trim(txtControl.value);
         
            var noOfdecimals= txtControl.noOfDecimals!=null?txtControl.noOfDecimals:"";
            var boolRange= txtControl.isRange!=null?txtControl.isRange:"";
            var minimum= txtControl.minimumValue!=null?txtControl.minimumValue:"";
            var maximum= txtControl.maximumValue!=null?txtControl.maximumValue:"";
            var labelName= txtControl.lableName!=null?txtControl.lableName:"";
            var measurements= txtControl.Measuments!=null?txtControl.Measuments:"";
            var dependentId= txtControl.dependentId!=null?txtControl.dependentId:"";
       
              if(Controlvalue!=null && Controlvalue.length>0)
                {
                   if(isNaN(Controlvalue))
                   {
                    txtControl.focus();
                    alert("Please enter only numerical values for " + labelName + ". No special characters and characters are allowed.");
                    return false;
                   }
                   else
                   {
          
                    var booldecimalflag;
                    var boolRangeflag;
                    if(parseInt(noOfdecimals,10)>0)
                    booldecimalflag=NumericMatchClick(Controlvalue,noOfdecimals,true);
                    else
                    booldecimalflag=NumericMatchClick(Controlvalue,noOfdecimals,false);	
                       
                     var boolRangeNew=boolRange=="true"?true:false;
                    
                      if(boolRangeNew)
                        {
                           boolRangeflag=CheckNumberRange(Controlvalue,minimum,maximum);                                        
                         }
                               
                         if(boolRangeNew)
                         {        
                          if(!booldecimalflag && !boolRangeflag)
                          {      
                          txtControl.focus();
                          
                          if(noOfdecimals == 0)
                            {
                             if (maximum == "infinite")
                             {                           
                             alert("Please enter only numerics greater than or equal to 0 without decimals for "+labelName+". No special characters and characters are allowed.");
                             }
                             else if (minimum == "infinite")
                             {                            
                             alert("Please enter only numerics less than or equal to "+ maximum +" without decimals for "+labelName+". No special characters and characters are allowed.");
                             }
                             else
                             {                            
                               alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" without decimals for "+labelName+". No special characters and characters are allowed.");
                             }
                            }
                            else if (maximum == "infinite")
                            {                           
                             alert("Please enter only numerics greater than or equal to 0 with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                             else if (minimum == "infinite")
                            {                            
                             alert("Please enter only numerics less than or equal to "+ maximum +" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                            else
                            {                            
                              alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                                   return false;
                          } 
                          else if(!booldecimalflag)
                          {
                            txtControl.focus();
                            if(noOfdecimals == 0)
                            {
//                            //"Please enter value without decimals."
//                            alert("Please enter value without decimals for "+labelName+".");
                                if (maximum == "infinite")
                                 {                           
                                 alert("Please enter only numerics greater than or equal to 0 without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else if (minimum == "infinite")
                                 {                            
                                 alert("Please enter only numerics less than or equal to "+ maximum +" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else
                                 {                            
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                            }
//                            else
//                            {
//                            alert("Please enter only "+noOfdecimals+" digits after decimal points for "+labelName+".");
//                            }
                            else if (maximum == "infinite")
                            {                           
                               alert("Please enter only numerics greater than or equal to 0 with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                             else if (minimum == "infinite")
                            {                            
                              alert("Please enter only numerics less than or equal to "+ maximum +" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                            else
                            {                            
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                             return false;
                          }
                          else if(!boolRangeflag)
                          { /// Validation for negative numbers;
                             txtControl.focus(); 
                            
                             if(noOfdecimals == 0)
                            {                            
                             //alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" without decimals for "+labelName+".");
                                 if (maximum == "infinite")
                                 {                           
                                 alert("Please enter only numerics greater than or equal to 0 without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else if (minimum == "infinite")
                                 {                            
                                 alert("Please enter only numerics less than or equal to "+ maximum +" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else
                                 {                            
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                            }
                             else if (maximum == "infinite")
                            {                           
                              alert("Please enter only numerics greater than or equal to 0 with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                             else if (minimum == "infinite")
                            {                           
                              alert("Please enter only numerics less than or equal to "+ maximum +" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                            else
                            {                           
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                            }
                             return false;
                          }
                         }
                         else 
                         {
                             if(!booldecimalflag)
                             {
                               txtControl.focus(); 
                               if(noOfdecimals == 0)
                               {
                               //alert("Please enter value without decimals for "+labelName+".");
                                 if (maximum == "infinite")
                                 {                           
                                 alert("Please enter only numerics greater than or equal to 0 without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else if (minimum == "infinite")
                                 {                            
                                 alert("Please enter only numerics less than or equal to "+ maximum +" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                                 else
                                 {                            
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" without decimals for "+labelName+". No special characters and characters are allowed.");
                                 }
                               }
                               else if (maximum == "infinite")
                               {                           
                                  alert("Please enter only numerics greater than or equal to 0 with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                               }
                             else if (minimum == "infinite")
                               {
                                  alert("Please enter only numerics less than or equal to "+ maximum +" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                               }
                            else
                               {                           
                                 alert("Please enter only "+minimum+measurements+"  to "+maximum+measurements+" with maximum "+noOfdecimals+" decimal points for "+labelName+". No special characters and characters are allowed.");
                               }
                                return false;
                              }
                          }	    	                  
                    }
                    
                    if(dependentId.length>0)
                    {
                     var txtChildControl=document.getElementById(dependentId);
                       if(txtChildControl!=null)
                        {
                          var ChildControlvalue=txtChildControl.value;
                              if(parseFloat(Controlvalue)<=parseFloat(ChildControlvalue))
                               {  
                                var childlabelName= txtChildControl.lableName!=null?txtChildControl.lableName:"";
                                  txtControl.focus(); 
                                  alert(labelName + " should be greater than "+childlabelName+".");
                                  return false;
                               }                                           
                        }
                    }                  
                }	  	 
        }
     }    	
    }
return true;

}

/// <summary>
///Slef Close Window .
/// </summary>        
      
function CloseWindow()
    {
      self.close();
    }
    
/// <summary>
///GetSeletectedItem for List Box .
/// </summary>
/// <param name="listBoxId">The List Box  Id.</param>       
/// <param name="controlId">The Control   Id.</param>
/// <param name="lblName">The Label Name.</param>
function GetSeletectedItem(listBoxId,controlId,lblName)
{
      var listbox=document.getElementById(listBoxId);
      var listLength = listbox.options.length; 
      var listvalue="";
       for (var itemIndex = 0; itemIndex < listLength; itemIndex++) 
       { 
            if (listbox.options[itemIndex].selected) 
            { 
              listvalue=listbox.options[itemIndex].text+";"+listbox.options[itemIndex].value;
              break;
            }
        }
       if(listvalue.length>0)
       {
         window.opener.AddNewItem(controlId,listvalue);
         self.close();

       }
       else
       {
        alert("Please Select the "+lblName +".");
       }    
}
    
/// <summary>
/// Populate TechnoSettings   DropDown Based on the TechnoClarification radio List .
/// </summary>
/// <param name="radiolistcontrol">The TechnoClarification radio List.</param>
/// <param name="ballydropId">The TechnoSettings DropDown box.</param>
 /// <param name="kleemdropId">TheTechnoSettings DropDown box.</param>    
function TechnoSettingsOnChange(radiolistcontrol,dropId,hidTecgContentId)
{

    var hidvarible=document.getElementById(hidTecgContentId);
    var dropValues=hidvarible.value;
    var temp=dropValues.split("|");
    var dropTechnoSettings = document.getElementById(dropId); 
    var i;
    for(i=dropTechnoSettings.options.length-1;i>0;i--)
        {
            dropTechnoSettings.remove(i);
        }
    var strBalley=radiolistcontrol.value+"_";  
    var count=1;
        for(i=0; i<temp.length;i++)
        {  var j=i+1;              
           if(temp[i].indexOf(strBalley)!=-1)
	        {
                 dropTechnoSettings.options[count] = new Option(temp[i].substring(temp[i].indexOf(strBalley)+strBalley.length), count) ;
                 count++;
	        }      

        }
}
	
/// <summary>
/// Loads Items  for Rad Combo Boxes.
/// </summary>
/// <param name="combo">The Rad combo </param>
/// <param name="eventArgs">The eventArqs.</param>      
function ItemsLoaded(combo, eventArgs)
{   

    if (combo.get_items().get_count() > 0)
    {
     // pre-select the first item
        combo.set_text(combo.get_items().getItem(0).get_text());
        combo.get_items().getItem(0).highlight();        
    }
    combo.commitChanges();
}

/// <summary>
/// Show and Hide SRP Blocks.
/// </summary>
/// <param name="control">The Controld ID </param>
/// <param name="tableId">The Table ID .</param>
function TableToggler(control,tableId)
{  
    var tableControl = document.getElementById(tableId); 
    if(tableControl)
    {
       if(control.checked==true)
       {
        tableControl.style.display = 'block';
       }
       else
       {
        tableControl.style.display = 'none';
       }
    }       	 
}


/// <summary>
/// Show and Hide SRP Blocks.
/// </summary>
/// <param name="control">The Controld ID </param>
/// <param name="tableId">The Table ID .</param>
function RowToggler(control,rowName,count)
{  

   for(i =1; i <= count; i ++)
   {
    var rowControl = document.getElementById(rowName+i); 
    if(rowControl)
    {
       if(control.checked==true)
       {
        rowControl.style.display = 'block';
       }
       else
       {
        rowControl.style.display = 'none';
       }
    }       	 
    
   }
}
    
/// <summary>
/// Open popup For ReserviorChronostrat.
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
function OpenChronostraticPopup(txtClientId)
{
/// 'status:no;dialogWidth:1000px;dialogHeight:1000px;dialogHide:true;help:no;scroll:yes'
 var listvalue=window.showModalDialog('/Pages/ReserviorChronostratPopup.aspx?ControlId='+txtClientId,null,'status:no;dialogWidth:1000px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes;maximize:1;minimize:1');
 AddChronostraticText(txtClientId,listvalue);    
}

/// <summary>
/// Open popup For ReserviorDepositional.
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
function OpenDepositionalPopup(txtClientId,hidControlId)
{                           
var listvalue=window.showModalDialog('/Pages/ReserviorDepositionalEnvPopup.aspx?ControlId='+txtClientId,null,'status:no;dialogWidth:800px;dialogHeight:500px;dialogHide:true;help:no;scroll:yes;maximize:1;minimize:1');
AddDepositionalText(txtClientId,listvalue,hidControlId);

}
    
/// <summary>
/// Clear the Text box values for Chronostratic
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
function ClearChronostraticValue(txtClientId)
{

var txtControl = document.getElementById(txtClientId);
if(txtControl)
{
txtControl.value="";
}
    if(document.getElementById(GetObjectID('hidDepositionalColumn',"input")))
     {
       document.getElementById(GetObjectID('hidDepositionalColumn',"input")).value = "";
     }
     // hidDepositionalValue
     if(document.getElementById(GetObjectID('hidDepositionalValue',"input")))
     {
       document.getElementById(GetObjectID('hidDepositionalValue',"input")).value = "";
     }
}

/// <summary>
///  Adds the ChronostraticTex
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
/// <param name="listvalue">The Value </param>
function AddChronostraticText(controlId,listvalue)
{
if(controlId)
{
     var txtControl = document.getElementById(controlId);
     if(txtControl && listvalue)
     {
        if(listvalue != undefined)
        {
        
        txtControl.value=listvalue;
        }
     }
}                              
}

/// <summary>
///  Adds the Depositional Text
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
/// <param name="listvalue">The Value </param>
function AddDepositionalText(controlId,listvalue,hidControlId)
{
if(controlId)
{
     var txtControl = document.getElementById(controlId);             
     if(listvalue)
     {
       listvalue = listvalue.split("|");
     }
     if(listvalue && listvalue.length == 3)
     {
     if(txtControl)
     {
        txtControl.value=listvalue[0];
     }
     if(document.getElementById(GetObjectID('hidDepositionalColumn',"input")))
     {
       document.getElementById(GetObjectID('hidDepositionalColumn',"input")).value = listvalue[2];
     }
     // hidDepositionalValue
     if(document.getElementById(GetObjectID('hidDepositionalValue',"input")))
     {
       document.getElementById(GetObjectID('hidDepositionalValue',"input")).value = listvalue[1];
     }
     
     }
}                          
}

/// <summary>
///  Adds selected value to Chronostratic text box
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
/// <param name="HidControlId">The HiddenControl Id </param>
function OnConfirmButtonClickOfChronostratic(controlId,HidControlId)
{

var listvalue=document.getElementById(HidControlId).value ;     

window.returnValue=listvalue; 
self.close() 

}

/// <summary>
///  Adds selected value to Depositional text box
/// </summary>
/// <param name="txtClientId">The Controld ID </param>
/// <param name="HidControlId">The HiddenControl Id </param>
function OnConfirmButtonClickOfDepositional(btncontrol,HidControlId,HidControlDepositonalValue)
{           
var selectedDepositionalLevel = btncontrol.DepositionalLevel;

selectedDepositionalLevel = selectedDepositionalLevel.split("#");
var DepositionalColumnName;
var DepositionalValue;
if(selectedDepositionalLevel != null && selectedDepositionalLevel.length == 2)
{
DepositionalValue =selectedDepositionalLevel[0];

if(selectedDepositionalLevel[1] != null)
{
 
   switch(selectedDepositionalLevel[1])
         {
           case "1":
             {
               DepositionalColumnName = "SlopeType";               
               break;
             }
           case "2":
             {
             DepositionalColumnName = "GrossDepositionalEnv";
               break;
             }
           case "3":
             {
              DepositionalColumnName = "DepositionalEnv";
               break;
             }
           case "4":
             {
              DepositionalColumnName = "SubDepositionalEnv";
               break;
             }
           case "5":
             {
              DepositionalColumnName = "Sedimentary Body";
               break;
             }
         }
}
}
var windowReturnValue = btncontrol.DepositionalText+"|" + DepositionalValue +"|" + DepositionalColumnName;

window.returnValue=windowReturnValue;
self.close() 

}
/// <summary>
///  Constructs table Based on the selection of Node
/// </summary>
/// <param name="sender">The sender </param>
/// <param name="eventArgs">The eventArgs </param> 
function ReservoirDepositionalOnNodeClicking(sender, eventArgs)
{ 
          
   var tableId="DepositinalEnvTreeViewSelectedNodeTable"
   var tbl=document.getElementById(tableId) ;       
   if(tbl)
   {
       while(tbl.hasChildNodes())
       {
         tbl.removeChild(tbl.lastChild);
       }
       var node = eventArgs.get_node();
        var btnConfirm= document.getElementById("btnConfirm");
             if(btnConfirm)
             {
               btnConfirm.style.display = 'block';
               btnConfirm.DepositionalText=node.get_text();
                btnConfirm.DepositionalLevel=node.get_value();
             }
             var btClose= document.getElementById("btClose");
             if(btClose)
             {
               btClose.style.display = 'block';
             }

       var nodeLevel= node.get_level();
        
         switch(nodeLevel)
         {

          case 0: AddRowsToTable(tbl,"Type",node.get_text());
                  break;
          case 1: AddRowsToTable(tbl,"Type",node._parent.get_text());
                  AddRowsToTable(tbl,"Gross Depositional Environment",node.get_text());
                  break;
          case 2:
                  AddRowsToTable(tbl,"Type",node._parent._parent.get_text());
                  AddRowsToTable(tbl,"Gross Depositional Environment",node._parent.get_text());
                  AddRowsToTable(tbl,"Depositional Environment",node.get_text());
                  break;
          case 3: 
                  AddRowsToTable(tbl,"Type",node._parent._parent._parent.get_text());
                  AddRowsToTable(tbl,"Gross Depositional Environment",node._parent._parent.get_text());
                  AddRowsToTable(tbl,"Depositional Environment",node._parent.get_text());
                  AddRowsToTable(tbl,"Sub Depositional Environment",node.get_text());
                  break;
          case 4: 
          
                  AddRowsToTable(tbl,"Type",node._parent._parent._parent._parent.get_text());
                  AddRowsToTable(tbl,"Gross Depositional Environment",node._parent._parent._parent.get_text());
                  AddRowsToTable(tbl,"Depositional Environment",node._parent._parent.get_text());
                  AddRowsToTable(tbl,"Sub Depositional Environment",node._parent.get_text());
                  AddRowsToTable(tbl,"Sedimentary Body",node.get_text());          
                  break;             
         }
  }
} 

/// <summary>
///  Add Rows to the tables based on passed parameters
/// </summary>
/// <param name="tbl">The Table </param>
/// <param name="leftNodeValue">The left TD Value </param>
/// <param name="rightNodeValue">The right TD Value </param>  
function AddRowsToTable(tbl,leftNodeValue,rightNodeValue)
 {      
 var lastRow = tbl.rows.length;
 var iteration = lastRow + 1;             
 var row = tbl.insertRow(lastRow); 
   row.style.backgroundColor="#EFEFEF";
   var spanTag = document.createElement("span");           
   spanTag.id = leftNodeValue+"span1";          
   spanTag.style.fontWeight="bold";
   spanTag.innerHTML =leftNodeValue
   var cellLeft = row.insertCell(0);
  
   cellLeft.appendChild(spanTag);   
   var rightspanTag = document.createElement("span");           
   rightspanTag.id = rightNodeValue+"span1";          
   rightspanTag.style.fontWeight="normal";
   rightspanTag.innerHTML =rightNodeValue
   var rightLeft = row.insertCell(1);
                             
   rightLeft.appendChild(rightspanTag);           
}

/// <summary>
///  Picks and assignHotspot to Lable 
/// </summary>
/// <param name="lableId">The lable</param>
/// <param name="hiddenControlId">The Hiddencontrol Id </param>
/// <param name="hotSpotName">The hotSpotNamee </param>         
function HotSpotClick(lableId,hiddenControlId,hotSpotName)
{

  var chronostratLable= document.getElementById(lableId); 
  if(chronostratLable)
  chronostratLable.innerText=hotSpotName;
  var chronostratHidControl= document.getElementById(hiddenControlId); 
  if(chronostratHidControl)
   chronostratHidControl.value=hotSpotName;
}

/// <summary>
/// Triggerd when Selected index of "Production Status" RadComboBox is changed.
/// Sets the selected item text as tooltip for the "Production Status" combobox
/// </summary>        
function OnProductionStatusSelectedIndexChanged(sender, eventArgs) 
{
sender.get_element().title = eventArgs.get_item().get_text();
}

/// <summary>
/// Function Called on click of SRP Info Icons.
/// Opens the info html pages in separate popup window.
/// </summary>
function OpenToolTipPopup(controlName)
{
    var windowFeatures ='height=200px,width=400px, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, directories=no, status=no, left=250, top=100,screenX=250,screenY=100';
    if(controlName == 'GrainSizeMean')
    {
    windowFeatures = 'height=630px,width=600px, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, directories=no, status=no, left=50, top=70,screenX=50,screenY=70';
    window.open ('/ToolTip%20Files/GrainSizeMean.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'LithostratGroup')
    {
    window.open ('/ToolTip%20Files/LithostratGroup.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'LithostratFormation')
    {
    window.open ('/ToolTip%20Files/LithostratFormation.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'LithostratMember')
    {
    window.open ('/ToolTip%20Files/LithostratMember.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'GasInitiallyInPlace')
    {
    window.open ('/ToolTip%20Files/GIIP.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'StockTankOilInitially')
    {
    window.open ('/ToolTip%20Files/STOIIP.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'OilGravity')
    {
    window.open ('/ToolTip%20Files/OilGravity.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'FieldName')
    {
    window.open ('/ToolTip%20Files/FieldName.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'OperationalEnv')
    {
    window.open ('/ToolTip%20Files/OperationalEnvironment.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'Operator')
    {
    window.open ('/ToolTip%20Files/OperatorCompany.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'TectonicSetting')
    {
    window.open ('/ToolTip%20Files/TectonicSetting.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'Country')
    {
    window.open ('/ToolTip%20Files/Country.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'ReserveMagnitudeOil')
    {
    window.open ('/ToolTip%20Files/ReserveMagnitude.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'PorosityMax')
    {
    window.open ('/ToolTip%20Files/Porosity.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'PermeabilityMax')
    {
    window.open ('/ToolTip%20Files/PermeabilityMax.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'OilInPlace')
    {
    window.open ('/ToolTip%20Files/OilInPlace.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'CondensateInPlace')
    {
    window.open ('/ToolTip%20Files/CondensateInPlace.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'CondensateRecoveryFactor')
    {
    window.open ('/ToolTip%20Files/CondensateRecoveryFactor.htm', 'newwindow',windowFeatures ); 
    }
    else if(controlName == 'BasinName')
    {
    window.open ('/ToolTip%20Files/BasinName.htm', 'newwindow',windowFeatures ); 
    }
    return false;
}

/// <summary>
/// Loads Items  for Rad Combo Boxes.
/// </summary>
/// <param name="combo">The Rad combo </param>
/// <param name="eventArqs">The eventArqs.</param>      
function OnClientSelectedIndexChanged(combo, eventArgs)
{   

if(eventArgs.get_item().get_text())
{
combo.set_text(eventArgs.get_item().get_text());
}
    combo.commitChanges();
    combo.showDropDown();
}

/// <summary>
/// Trims left and right white spaces in user entered text.
/// </summary>
/// <param name="ctrlValue">Input text</param>
function Trim(ctrlValue)
{
if(ctrlValue.length > 0)
{
	while(ctrlValue.charAt(0)==' ')
	{
		ctrlValue=ctrlValue.substring(1,ctrlValue.length );
	}
	while(ctrlValue.charAt(ctrlValue.length-1)==' ')
	{
		ctrlValue=ctrlValue.substring(0,ctrlValue.length-1)
    }
}
return ctrlValue;
}

/// <summary>
/// Loads Items  for Rad Combo Boxes.
/// </summary>
/// <param name="combo">The Rad combo </param>
/// <param name="eventArgs">The eventArqs.</param>      
function OnClientItemRequesting(combo, eventArgs)
{ 
//alert(combo.get_items().get_count());
//alert(eventArgs.get_text());
    if (combo.get_items().get_count() > 0)
    {
     // Cancel the ItemRequest if No Records Found / only Select is present in RadComboBox items
      if (eventArgs.get_text().length < 1)
        {
           eventArgs.set_cancel(true); 
        }
      else if (eventArgs.get_text() == "No Records Found")
        {
           eventArgs.set_cancel(true); 
        }
//        else 
//        {
//         
//        if (combo.get_items().get_count() == 1 && combo.get_items().getItem(0).get_text() == "---Select---")
//        {
//         alert('event cancelled');
//        eventArgs.set_cancel(true); 
//        }
//        }
    }       
}