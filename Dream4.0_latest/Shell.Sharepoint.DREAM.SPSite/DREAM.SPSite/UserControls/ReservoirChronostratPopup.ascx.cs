#region Shell Copyright.2010
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: ReservoirChronostratPopup.ascx.cs 
#endregion




using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;

namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    /// <summary>
    /// Reservoir Chronostrat UI Screen for Reservoir Adv Search 
    /// </summary>
    public partial class ReservoirChronostratPopup : UIControlHandler
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            string strControlId = string.Empty;
            if (Request.QueryString[SRPADVPOPUPID] != null)
                {
                    if (Request.QueryString[SRPADVPOPUPID].ToString().Length > 0)
                    {
                        strControlId = Request.QueryString[SRPADVPOPUPID].ToString();
                    }
               }
            StringBuilder strConfrimOnclickJavascript = new StringBuilder();
            strConfrimOnclickJavascript.Append("javascript:OnConfirmButtonClickOfChronostratic('");
            strConfrimOnclickJavascript.Append(strControlId);
            strConfrimOnclickJavascript.Append("','");
            strConfrimOnclickJavascript.Append(hidFldChronostrat.ClientID);
            strConfrimOnclickJavascript.Append("');return false;");
            btnConfirm.Attributes.Add(JAVASCRIPTONCLICK, strConfrimOnclickJavascript.ToString());

            RectangleHotSpot rectHotSpotLutetian = new RectangleHotSpot();
            rectHotSpotLutetian.Left = 171;
            rectHotSpotLutetian.Top = 167;
            rectHotSpotLutetian.Right = 317;
            rectHotSpotLutetian.Bottom = 186;
            rectHotSpotLutetian.NavigateUrl = AttachJavaScriptForHotSpot("Lutetian (LT)");           
            rectHotSpotLutetian.AlternateText = "Lutetian (LT)";
            
            imgMapChronostrat.HotSpots.Add(rectHotSpotLutetian);

            RectangleHotSpot rectHotSpotMississipian = new RectangleHotSpot();
            rectHotSpotMississipian.Top = 188;
            rectHotSpotMississipian.Left = 503;
            rectHotSpotMississipian.Bottom = 269;
            rectHotSpotMississipian.Right = 575;
            rectHotSpotMississipian.NavigateUrl = AttachJavaScriptForHotSpot("Mississipian (MS)");
            rectHotSpotMississipian.AlternateText = "Mississipian (MS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMississipian);

            RectangleHotSpot  rectHotSpotSixthStage = new RectangleHotSpot();
            rectHotSpotSixthStage.Left = 593;
            rectHotSpotSixthStage.Top = 540;
            rectHotSpotSixthStage.Right = 725;
            rectHotSpotSixthStage.Bottom = 555;
            rectHotSpotSixthStage.NavigateUrl = AttachJavaScriptForHotSpot("Sixth Stage (EE6)");
            rectHotSpotSixthStage.AlternateText = "Sixth Stage (EE6)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSixthStage);

            RectangleHotSpot  rectHotSpotTremadocian = new RectangleHotSpot();
            rectHotSpotTremadocian.Left = 592;
            rectHotSpotTremadocian.Top = 520;
            rectHotSpotTremadocian.Right = 724;
            rectHotSpotTremadocian.Bottom = 540;
            rectHotSpotTremadocian.NavigateUrl = AttachJavaScriptForHotSpot("Tremadocian (TM)");
            rectHotSpotTremadocian.AlternateText = "Tremadocian (TM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTremadocian);

            RectangleHotSpot  rectHotSpotSecondStage = new RectangleHotSpot();
            rectHotSpotSecondStage.Left = 593;
            rectHotSpotSecondStage.Top = 506;
            rectHotSpotSecondStage.Right = 725;
            rectHotSpotSecondStage.Bottom = 519;
            rectHotSpotSecondStage.NavigateUrl = AttachJavaScriptForHotSpot("Second Stage (OO2)");
            rectHotSpotSecondStage.AlternateText = "Second Stage (OO2)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSecondStage);


            RectangleHotSpot  rectHotSpotThirdStage = new RectangleHotSpot();
            rectHotSpotThirdStage.Left = 593;
            rectHotSpotThirdStage.Top = 496;
            rectHotSpotThirdStage.Right = 725;
            rectHotSpotThirdStage.Bottom = 505;
            rectHotSpotThirdStage.NavigateUrl = AttachJavaScriptForHotSpot("Third Stage (OO3)");
            rectHotSpotThirdStage.AlternateText = "Third Stage (OO3)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotThirdStage);

            RectangleHotSpot  rectHotSpotDarriwilian = new RectangleHotSpot();
            rectHotSpotDarriwilian.Left = 592;
            rectHotSpotDarriwilian.Top = 520;
            rectHotSpotDarriwilian.Right = 724;
            rectHotSpotDarriwilian.Bottom = 540;
            rectHotSpotDarriwilian.NavigateUrl = AttachJavaScriptForHotSpot("Darriwilian (DW)");
            rectHotSpotDarriwilian.NavigateUrl = "Darriwilian (DW)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotDarriwilian);

            RectangleHotSpot rectHotSpotFifthStage005 = new RectangleHotSpot();
            rectHotSpotFifthStage005.Left = 593;
            rectHotSpotFifthStage005.Top = 469;
            rectHotSpotFifthStage005.Right = 724;
            rectHotSpotFifthStage005.Bottom = 482;
            rectHotSpotFifthStage005.NavigateUrl = AttachJavaScriptForHotSpot("Fifth Stage (OO5)");
            rectHotSpotFifthStage005.AlternateText = "Fifth Stage (OO5)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotFifthStage005);

            RectangleHotSpot  rectHotSpotSixthStageOO6 = new RectangleHotSpot();
            rectHotSpotSixthStageOO6.Left = 593;
            rectHotSpotSixthStageOO6.Top = 457;
            rectHotSpotSixthStageOO6.Right = 725;
            rectHotSpotSixthStageOO6.Bottom = 469;
            rectHotSpotSixthStageOO6.NavigateUrl = AttachJavaScriptForHotSpot("Sixth Stage (OO6)");
            rectHotSpotSixthStageOO6.AlternateText = "Sixth Stage (OO6)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSixthStageOO6);

            RectangleHotSpot  rectHotSpotHimantian = new RectangleHotSpot();
            rectHotSpotHimantian.Left = 593;
            rectHotSpotHimantian.Top = 446;
            rectHotSpotHimantian.Right = 725;
            rectHotSpotHimantian.Bottom = 457;
            rectHotSpotHimantian.NavigateUrl = AttachJavaScriptForHotSpot("Himantian (HI)");
            rectHotSpotHimantian.AlternateText = "Himantian (HI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotHimantian);

            RectangleHotSpot  rectHotSpotLowerCambrian = new RectangleHotSpot();
            rectHotSpotLowerCambrian.Left = 504;
            rectHotSpotLowerCambrian.Top = 591;
            rectHotSpotLowerCambrian.Right = 725;
            rectHotSpotLowerCambrian.Bottom = 659;
            rectHotSpotLowerCambrian.NavigateUrl = AttachJavaScriptForHotSpot("Lower Cambrian (EL)");
            rectHotSpotLowerCambrian.AlternateText = "Lower Cambrian (EL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerCambrian);

            RectangleHotSpot  rectHotSpotMiddleCambrian = new RectangleHotSpot();
            rectHotSpotMiddleCambrian.Left = 503;
            rectHotSpotMiddleCambrian.Top = 566;
            rectHotSpotMiddleCambrian.Right = 725;
            rectHotSpotMiddleCambrian.Bottom = 591;
            rectHotSpotMiddleCambrian.NavigateUrl = AttachJavaScriptForHotSpot("Middle Cambrian (EM)");
            rectHotSpotMiddleCambrian.AlternateText = "Middle Cambrian (EM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiddleCambrian);

            RectangleHotSpot  rectHotSpotFurongian = new RectangleHotSpot();
            rectHotSpotFurongian.Left = 504;
            rectHotSpotFurongian.Top = 540;
            rectHotSpotFurongian.Right = 592;
            rectHotSpotFurongian.Bottom = 566;
            rectHotSpotFurongian.NavigateUrl = AttachJavaScriptForHotSpot("Furongian (FU)");
            rectHotSpotFurongian.AlternateText = "Furongian (FU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotFurongian);

            RectangleHotSpot  rectHotSpotCambrian = new RectangleHotSpot();
            rectHotSpotCambrian.Left = 462;
            rectHotSpotCambrian.Top = 540;
            rectHotSpotCambrian.Right = 503;
            rectHotSpotCambrian.Bottom = 661;
            rectHotSpotCambrian.NavigateUrl = AttachJavaScriptForHotSpot("Cambrian (EE)");
            rectHotSpotCambrian.AlternateText = "Cambrian (EE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCambrian);

            RectangleHotSpot rectHotSpotLowerOol = new RectangleHotSpot();
            rectHotSpotLowerOol.Left = 504;
            rectHotSpotLowerOol.Top = 506;
            rectHotSpotLowerOol.Right = 592;
            rectHotSpotLowerOol.Bottom = 540;
            rectHotSpotLowerOol.NavigateUrl = AttachJavaScriptForHotSpot("Lower (OOL)");
            rectHotSpotLowerOol.AlternateText = "Lower (OOL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerOol);

            RectangleHotSpot  rectHotSpotMiddleOOM = new RectangleHotSpot();
            rectHotSpotMiddleOOM.Left = 503;
            rectHotSpotMiddleOOM.Top = 483;
            rectHotSpotMiddleOOM.Right = 593;
            rectHotSpotMiddleOOM.Bottom = 506;
            rectHotSpotMiddleOOM.NavigateUrl = AttachJavaScriptForHotSpot("Middle (OOM)");
            rectHotSpotMiddleOOM.AlternateText = "Middle (OOM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiddleOOM);

            RectangleHotSpot  rectHotSpotUpperOrdovician = new RectangleHotSpot();
            rectHotSpotUpperOrdovician.Left = 504;
            rectHotSpotUpperOrdovician.Top = 446;
            rectHotSpotUpperOrdovician.Right = 593;
            rectHotSpotUpperOrdovician.Bottom = 483;
            rectHotSpotUpperOrdovician.NavigateUrl = AttachJavaScriptForHotSpot("Upper Ordovician (OOU)");
            rectHotSpotUpperOrdovician.AlternateText = "Upper Ordovician (OOU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperOrdovician);

            RectangleHotSpot  rectHotSpotAeronianRhuddian = new RectangleHotSpot();
            rectHotSpotAeronianRhuddian.Left = 593;
            rectHotSpotAeronianRhuddian.Top = 429;
            rectHotSpotAeronianRhuddian.Right = 725;
            rectHotSpotAeronianRhuddian.Bottom = 446;
            rectHotSpotAeronianRhuddian.NavigateUrl = AttachJavaScriptForHotSpot("Aeronian (AE)/Rhuddian (RD)");
            rectHotSpotAeronianRhuddian.AlternateText = "Aeronian (AE)/Rhuddian (RD)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAeronianRhuddian);

            RectangleHotSpot  rectHotSpotTelychian = new RectangleHotSpot();
            rectHotSpotTelychian.Left = 593;
            rectHotSpotTelychian.Top = 413;
            rectHotSpotTelychian.Right = 725;
            rectHotSpotTelychian.Bottom = 446;
            rectHotSpotTelychian.NavigateUrl = AttachJavaScriptForHotSpot("Telychian (TE)");
            rectHotSpotTelychian.AlternateText = "Telychian (TE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTelychian);

            RectangleHotSpot  rectHotSpotLlandovery = new RectangleHotSpot();
            rectHotSpotLlandovery.Left = 504;
            rectHotSpotLlandovery.Top = 413;
            rectHotSpotLlandovery.Right = 592;
            rectHotSpotLlandovery.Bottom = 446;
            rectHotSpotLlandovery.NavigateUrl = AttachJavaScriptForHotSpot("Llandovery (LO)");
            rectHotSpotLlandovery.AlternateText = "Llandovery (LO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLlandovery);

            RectangleHotSpot  rectHotSpotWenlock = new RectangleHotSpot();
            rectHotSpotWenlock.Left = 504;
            rectHotSpotWenlock.Top = 403;
            rectHotSpotWenlock.Right = 725;
            rectHotSpotWenlock.Bottom = 413;
            rectHotSpotWenlock.NavigateUrl = AttachJavaScriptForHotSpot("Wenlock (WN)");
            rectHotSpotWenlock.AlternateText = "Wenlock (WN)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotWenlock);

            RectangleHotSpot  rectHotSpotLudlow = new RectangleHotSpot();
            rectHotSpotLudlow.Left = 504;
            rectHotSpotLudlow.Top = 394;
            rectHotSpotLudlow.Right = 725;
            rectHotSpotLudlow.Bottom = 403;
            rectHotSpotLudlow.NavigateUrl = AttachJavaScriptForHotSpot("Ludlow (LD)");
            rectHotSpotLudlow.AlternateText = "Ludlow (LD)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLudlow);

            RectangleHotSpot  rectHotSpotPridoli = new RectangleHotSpot();
            rectHotSpotPridoli.Left = 504;
            rectHotSpotPridoli.Top = 388;
            rectHotSpotPridoli.Right = 724;
            rectHotSpotPridoli.Bottom = 394;
            rectHotSpotPridoli.NavigateUrl = AttachJavaScriptForHotSpot("Pridoli (PD)");
            rectHotSpotPridoli.AlternateText = "Pridoli (PD)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPridoli);

            RectangleHotSpot  rectHotSpotLowerDevonian = new RectangleHotSpot();
            rectHotSpotLowerDevonian.Left = 504;
            rectHotSpotLowerDevonian.Top = 351;
            rectHotSpotLowerDevonian.Right = 593;
            rectHotSpotLowerDevonian.Bottom = 388;
            rectHotSpotLowerDevonian.NavigateUrl = AttachJavaScriptForHotSpot("Lower Devonian (DL)");
            rectHotSpotLowerDevonian.AlternateText = "Lower Devonian (DL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerDevonian);

            RectangleHotSpot  rectHotSpotSilurian = new RectangleHotSpot();
            rectHotSpotSilurian.Left = 462;
            rectHotSpotSilurian.Top = 390;
            rectHotSpotSilurian.Right = 503;
            rectHotSpotSilurian.Bottom = 446;
            rectHotSpotSilurian.NavigateUrl = AttachJavaScriptForHotSpot("Silurian (SS)");
            rectHotSpotSilurian.AlternateText = "Silurian (SS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSilurian);

            RectangleHotSpot  rectHotSpotDevonian = new RectangleHotSpot();
            rectHotSpotDevonian.Left = 462;
            rectHotSpotDevonian.Top = 270;
            rectHotSpotDevonian.Right = 503;
            rectHotSpotDevonian.Bottom = 390;
            rectHotSpotDevonian.NavigateUrl = AttachJavaScriptForHotSpot("Devonian (DD)");
            rectHotSpotDevonian.AlternateText = "Devonian (DD)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotDevonian);

            RectangleHotSpot  rectHotSpotLochkovian = new RectangleHotSpot();
            rectHotSpotLochkovian.Left = 593;
            rectHotSpotLochkovian.Top = 379;
            rectHotSpotLochkovian.Right = 725;
            rectHotSpotLochkovian.Bottom = 388;
            rectHotSpotLochkovian.NavigateUrl = AttachJavaScriptForHotSpot("Lochkovian (LK)");
            rectHotSpotLochkovian.AlternateText = "Lochkovian (LK)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLochkovian);

            RectangleHotSpot  rectHotSpotPragian = new RectangleHotSpot();
            rectHotSpotPragian.Left = 593;
            rectHotSpotPragian.Top = 369;
            rectHotSpotPragian.Right = 725;
            rectHotSpotPragian.Bottom = 379;
            rectHotSpotPragian.NavigateUrl = AttachJavaScriptForHotSpot("Pragian (PR)");
            rectHotSpotPragian.AlternateText = "Pragian (PR)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPragian);

            RectangleHotSpot  rectHotSpotEmsian = new RectangleHotSpot();
            rectHotSpotEmsian.Left = 593;
            rectHotSpotEmsian.Top = 351;
            rectHotSpotEmsian.Right = 725;
            rectHotSpotEmsian.Bottom = 368;
            rectHotSpotEmsian.NavigateUrl = AttachJavaScriptForHotSpot("Emsian (ES)");
            rectHotSpotEmsian.AlternateText = "Emsian (ES)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotEmsian);

            RectangleHotSpot  rectHotSpotEifelian = new RectangleHotSpot();
            rectHotSpotEifelian.Left = 593;
            rectHotSpotEifelian.Top = 341;
            rectHotSpotEifelian.Right = 724;
            rectHotSpotEifelian.Bottom = 350;
            rectHotSpotEifelian.NavigateUrl = AttachJavaScriptForHotSpot("Eifelian (EF)");
            rectHotSpotEifelian.AlternateText = "Eifelian (EF)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotEifelian);

            RectangleHotSpot  rectHotSpotGivetian = new RectangleHotSpot();
            rectHotSpotGivetian.Left = 593;
            rectHotSpotGivetian.Top = 326;
            rectHotSpotGivetian.Right = 725;
            rectHotSpotGivetian.Bottom = 341;
            rectHotSpotGivetian.NavigateUrl = AttachJavaScriptForHotSpot("Givetian (GI)");
            rectHotSpotGivetian.AlternateText = "Givetian (GI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotGivetian);

            RectangleHotSpot  rectHotSpotFrasnian = new RectangleHotSpot();
            rectHotSpotFrasnian.Left = 593;
            rectHotSpotFrasnian.Top = 303;
            rectHotSpotFrasnian.Right = 725;
            rectHotSpotFrasnian.Bottom = 325;
            rectHotSpotFrasnian.NavigateUrl = AttachJavaScriptForHotSpot("Frasnian (FS)");
            rectHotSpotFrasnian.AlternateText = "Frasnian (FS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotFrasnian);

            RectangleHotSpot  rectHotSpotFamennian = new RectangleHotSpot();
            rectHotSpotFamennian.Left = 593;
            rectHotSpotFamennian.Top = 270;
            rectHotSpotFamennian.Right = 725;
            rectHotSpotFamennian.Bottom = 303;
            rectHotSpotFamennian.NavigateUrl = AttachJavaScriptForHotSpot("Famennian (FA)");
            rectHotSpotFamennian.AlternateText = "Famennian (FA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotFamennian);

            RectangleHotSpot  rectHotSpotMiddleDevonism = new RectangleHotSpot();
            rectHotSpotMiddleDevonism.Left = 504;
            rectHotSpotMiddleDevonism.Top = 326;
            rectHotSpotMiddleDevonism.Right = 592;
            rectHotSpotMiddleDevonism.Bottom = 350;
            rectHotSpotMiddleDevonism.NavigateUrl = AttachJavaScriptForHotSpot("Middle Devonism (DM)");
            rectHotSpotMiddleDevonism.AlternateText = "Middle Devonism (DM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiddleDevonism);


            RectangleHotSpot  rectHotSpotUpperDevonian = new RectangleHotSpot();
            rectHotSpotUpperDevonian.Left = 504;
            rectHotSpotUpperDevonian.Top = 270;
            rectHotSpotUpperDevonian.Right = 593;
            rectHotSpotUpperDevonian.Bottom = 326;
            rectHotSpotUpperDevonian.NavigateUrl = AttachJavaScriptForHotSpot("Upper Devonian (UD)");
            rectHotSpotUpperDevonian.AlternateText = "Upper Devonian (UD)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperDevonian);


            RectangleHotSpot  rectHotSpotTournaisian = new RectangleHotSpot();
            rectHotSpotTournaisian.Left = 577;
            rectHotSpotTournaisian.Top = 243;
            rectHotSpotTournaisian.Right = 725;
            rectHotSpotTournaisian.Bottom = 270;
            rectHotSpotTournaisian.NavigateUrl = AttachJavaScriptForHotSpot("Tournaisian (TO)");
            rectHotSpotTournaisian.AlternateText = "Tournaisian (TO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTournaisian);

            RectangleHotSpot  rectHotSpotVisean = new RectangleHotSpot();
            rectHotSpotVisean.Left = 577;
            rectHotSpotVisean.Top = 204;
            rectHotSpotVisean.Right = 725;
            rectHotSpotVisean.Bottom = 243;
            rectHotSpotVisean.NavigateUrl = AttachJavaScriptForHotSpot("Visean (VI)");
            rectHotSpotVisean.AlternateText = "Visean (VI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotVisean);


            RectangleHotSpot  rectHotSpotSerpukhovian = new RectangleHotSpot();
            rectHotSpotSerpukhovian.Left = 577;
            rectHotSpotSerpukhovian.Top = 188;
            rectHotSpotSerpukhovian.Right = 725;
            rectHotSpotSerpukhovian.Bottom = 204;
            rectHotSpotSerpukhovian.NavigateUrl = AttachJavaScriptForHotSpot("Serpukhovian (SP)");
            rectHotSpotSerpukhovian.AlternateText = "Serpukhovian (SP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSerpukhovian);


            RectangleHotSpot  rectHotSpotBashkirian = new RectangleHotSpot();
            rectHotSpotBashkirian.Left = 593;
            rectHotSpotBashkirian.Top = 172;
            rectHotSpotBashkirian.Right = 725;
            rectHotSpotBashkirian.Bottom = 187;
            rectHotSpotBashkirian.NavigateUrl = AttachJavaScriptForHotSpot("Bashkirian (BK)");
            rectHotSpotBashkirian.AlternateText = "Bashkirian (BK)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBashkirian);


            RectangleHotSpot  rectHotSpotMoscovian = new RectangleHotSpot();
            rectHotSpotMoscovian.Left = 593;
            rectHotSpotMoscovian.Top = 162;
            rectHotSpotMoscovian.Right = 725;
            rectHotSpotMoscovian.Bottom = 172;
            rectHotSpotMoscovian.NavigateUrl = AttachJavaScriptForHotSpot("Moscovian (MO)");
            rectHotSpotMoscovian.AlternateText = "Moscovian (MO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMoscovian);


            RectangleHotSpot  rectHotSpotKasimovian = new RectangleHotSpot();
            rectHotSpotKasimovian.Left = 593;
            rectHotSpotKasimovian.Top = 156;
            rectHotSpotKasimovian.Right = 725;
            rectHotSpotKasimovian.Bottom = 162;
            rectHotSpotKasimovian.NavigateUrl = AttachJavaScriptForHotSpot("Kasimovian (KS)");
            rectHotSpotKasimovian.AlternateText = "Kasimovian (KS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotKasimovian);


            RectangleHotSpot  rectHotSpotGzhelian = new RectangleHotSpot();
            rectHotSpotGzhelian.Left = 593;
            rectHotSpotGzhelian.Top = 147;
            rectHotSpotGzhelian.Right = 724;
            rectHotSpotGzhelian.Bottom = 155;
            rectHotSpotGzhelian.NavigateUrl = AttachJavaScriptForHotSpot("Gzhelian (GZ)");
            rectHotSpotGzhelian.AlternateText = "Gzhelian (GZ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotGzhelian);


            RectangleHotSpot  rectHotSpotLowerPennsylvanian = new RectangleHotSpot();
            rectHotSpotLowerPennsylvanian.Left = 577;
            rectHotSpotLowerPennsylvanian.Top = 172;
            rectHotSpotLowerPennsylvanian.Right = 592;
            rectHotSpotLowerPennsylvanian.Bottom = 187;
            rectHotSpotLowerPennsylvanian.NavigateUrl = AttachJavaScriptForHotSpot("Lower Pennsylvanian (PE)");
            rectHotSpotLowerPennsylvanian.AlternateText = "Lower Pennsylvanian (PE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerPennsylvanian);


            RectangleHotSpot  rectHotSpotPennsylvanian = new RectangleHotSpot();
            rectHotSpotPennsylvanian.Left = 504;
            rectHotSpotPennsylvanian.Top = 146;
            rectHotSpotPennsylvanian.Right = 577;
            rectHotSpotPennsylvanian.Bottom = 188;
            rectHotSpotPennsylvanian.NavigateUrl = AttachJavaScriptForHotSpot("Pennsylvanian (PE)");
            rectHotSpotPennsylvanian.AlternateText = "Pennsylvanian (PE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPennsylvanian);


            RectangleHotSpot  rectHotSpotAsselian = new RectangleHotSpot();
            rectHotSpotAsselian.Left = 593;
            rectHotSpotAsselian.Top = 138;
            rectHotSpotAsselian.Right = 725;
            rectHotSpotAsselian.Bottom = 146;
            rectHotSpotAsselian.NavigateUrl = AttachJavaScriptForHotSpot("Asselian (AS)");
            rectHotSpotAsselian.AlternateText = "Asselian (AS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAsselian);


            RectangleHotSpot  rectHotSpotSakmarian = new RectangleHotSpot();
            rectHotSpotSakmarian.Left = 593;
            rectHotSpotSakmarian.Top = 115;
            rectHotSpotSakmarian.Right = 725;
            rectHotSpotSakmarian.Bottom = 138;
            rectHotSpotSakmarian.NavigateUrl = AttachJavaScriptForHotSpot("Sakmarian (SR)");
            rectHotSpotSakmarian.AlternateText = "Sakmarian (SR)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSakmarian);


            RectangleHotSpot  rectHotSpotArtinkskian = new RectangleHotSpot();
            rectHotSpotArtinkskian.Left = 592;
            rectHotSpotArtinkskian.Top = 103;
            rectHotSpotArtinkskian.Right = 727;
            rectHotSpotArtinkskian.Bottom = 115;
            rectHotSpotArtinkskian.NavigateUrl = AttachJavaScriptForHotSpot("Artinkskian");
            rectHotSpotArtinkskian.AlternateText = "Artinkskian";
            imgMapChronostrat.HotSpots.Add(rectHotSpotArtinkskian);


            RectangleHotSpot  rectHotSpotPermian = new RectangleHotSpot();
            rectHotSpotPermian.Left = 593;
            rectHotSpotPermian.Top = 88;
            rectHotSpotPermian.Right = 725;
            rectHotSpotPermian.Bottom = 103;
            rectHotSpotPermian.NavigateUrl = AttachJavaScriptForHotSpot("Permian (KG)");
            rectHotSpotPermian.AlternateText = "Permian (KG)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPermian);


            RectangleHotSpot  rectHotSpotWordian = new RectangleHotSpot();
            rectHotSpotWordian.Left = 593;
            rectHotSpotWordian.Top = 80;
            rectHotSpotWordian.Right = 725;
            rectHotSpotWordian.Bottom = 87;
            rectHotSpotWordian.NavigateUrl = AttachJavaScriptForHotSpot("Wordian (WO)/Roadian (RO)");
            rectHotSpotWordian.AlternateText = "Wordian (WO)/Roadian (RO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotWordian);

            RectangleHotSpot  rectHotSpotCapitanian = new RectangleHotSpot();
            rectHotSpotCapitanian.Left = 593;
            rectHotSpotCapitanian.Top = 66;
            rectHotSpotCapitanian.Right = 725;
            rectHotSpotCapitanian.Bottom = 80;
            rectHotSpotCapitanian.NavigateUrl = AttachJavaScriptForHotSpot("Capitanian (CP)");
            rectHotSpotCapitanian.AlternateText = "Capitanian (CP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCapitanian);

            RectangleHotSpot  rectHotSpotWuchiapingian = new RectangleHotSpot();
            rectHotSpotWuchiapingian.Left = 593;
            rectHotSpotWuchiapingian.Top = 53;
            rectHotSpotWuchiapingian.Right = 725;
            rectHotSpotWuchiapingian.Bottom = 66;
            rectHotSpotWuchiapingian.NavigateUrl = AttachJavaScriptForHotSpot("Wuchiapingian (WU)");
            rectHotSpotWuchiapingian.AlternateText = "Wuchiapingian (WU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotWuchiapingian);

            RectangleHotSpot  rectHotSpotChanghsingian = new RectangleHotSpot();
            rectHotSpotChanghsingian.Left = 593;
            rectHotSpotChanghsingian.Top = 46;
            rectHotSpotChanghsingian.Right = 725;
            rectHotSpotChanghsingian.Bottom = 53;
            rectHotSpotChanghsingian.NavigateUrl = AttachJavaScriptForHotSpot("Changhsingian (CS)");
            rectHotSpotChanghsingian.AlternateText = "Changhsingian (CS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotChanghsingian);

            RectangleHotSpot  rectHotSpotCisuralian = new RectangleHotSpot();
            rectHotSpotCisuralian.Left = 504;
            rectHotSpotCisuralian.Top = 88;
            rectHotSpotCisuralian.Right = 593;
            rectHotSpotCisuralian.Bottom = 146;
            rectHotSpotCisuralian.NavigateUrl = AttachJavaScriptForHotSpot("Cisuralian (CI)");
            rectHotSpotCisuralian.AlternateText = "Cisuralian (CI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCisuralian);

            RectangleHotSpot  rectHotSpotGuadalupian = new RectangleHotSpot();
            rectHotSpotGuadalupian.Left = 503;
            rectHotSpotGuadalupian.Top = 66;
            rectHotSpotGuadalupian.Right = 593;
            rectHotSpotGuadalupian.Bottom = 88;
            rectHotSpotGuadalupian.NavigateUrl = AttachJavaScriptForHotSpot("Guadalupian (GP)");
            rectHotSpotGuadalupian.AlternateText = "Guadalupian (GP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotGuadalupian);

            RectangleHotSpot  rectHotSpotLopingian = new RectangleHotSpot();
            rectHotSpotLopingian.Left = 504;
            rectHotSpotLopingian.Top = 47;
            rectHotSpotLopingian.Right = 593;
            rectHotSpotLopingian.Bottom = 66;
            rectHotSpotLopingian.NavigateUrl = AttachJavaScriptForHotSpot("Lopingian (LP)");
            rectHotSpotLopingian.AlternateText = "Lopingian (LP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLopingian);

            RectangleHotSpot  rectHotSpotCarboniferous = new RectangleHotSpot();
            rectHotSpotCarboniferous.Left = 462;
            rectHotSpotCarboniferous.Top = 147;
            rectHotSpotCarboniferous.Right = 503;
            rectHotSpotCarboniferous.Bottom = 270;
            rectHotSpotCarboniferous.NavigateUrl = AttachJavaScriptForHotSpot("Carboniferous (CC)");
            rectHotSpotCarboniferous.AlternateText = "Carboniferous (CC)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCarboniferous);

            RectangleHotSpot  rectHotSpotPermianPP = new RectangleHotSpot();
            rectHotSpotPermianPP.Left = 462;
            rectHotSpotPermianPP.Top = 47;
            rectHotSpotPermianPP.Right = 504;
            rectHotSpotPermianPP.Bottom = 146;
            rectHotSpotPermianPP.NavigateUrl = AttachJavaScriptForHotSpot("Permian (PP)");
            rectHotSpotPermianPP.AlternateText = "Permian (PP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPermianPP);

            RectangleHotSpot  rectHotSpotPaleozoic = new RectangleHotSpot();
            rectHotSpotPaleozoic.Left = 420;
            rectHotSpotPaleozoic.Top = 47;
            rectHotSpotPaleozoic.Right = 462;
            rectHotSpotPaleozoic.Bottom = 659;
            rectHotSpotPaleozoic.NavigateUrl = AttachJavaScriptForHotSpot("Paleozoic (PZ)");
            rectHotSpotPaleozoic.AlternateText = "Paleozoic (PZ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPaleozoic);

            RectangleHotSpot  rectHotSpotInduan = new RectangleHotSpot();
            rectHotSpotInduan.Left = 184;
            rectHotSpotInduan.Top = 761;
            rectHotSpotInduan.Right = 319;
            rectHotSpotInduan.Bottom = 770;
            rectHotSpotInduan.NavigateUrl = AttachJavaScriptForHotSpot("Induan (IN)");
            rectHotSpotInduan.AlternateText = "Induan (IN)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotInduan);

            RectangleHotSpot  rectHotSpotOlenakian = new RectangleHotSpot();
            rectHotSpotOlenakian.Left = 184;
            rectHotSpotOlenakian.Top = 751;
            rectHotSpotOlenakian.Right = 317;
            rectHotSpotOlenakian.Bottom = 760;
            rectHotSpotOlenakian.NavigateUrl = AttachJavaScriptForHotSpot("Olenakian (OK)");
            rectHotSpotOlenakian.AlternateText = "Olenakian (OK)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotOlenakian);

            RectangleHotSpot  rectHotSpotAnisian = new RectangleHotSpot();
            rectHotSpotAnisian.Left = 184;
            rectHotSpotAnisian.Top = 727;
            rectHotSpotAnisian.Right = 318;
            rectHotSpotAnisian.Bottom = 751;
            rectHotSpotAnisian.NavigateUrl = AttachJavaScriptForHotSpot("Anisian (AN)");
            rectHotSpotAnisian.AlternateText = "Anisian (AN)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAnisian);


            RectangleHotSpot  rectHotSpotLadinian = new RectangleHotSpot();
            rectHotSpotLadinian.Left = 184;
            rectHotSpotLadinian.Top = 703;
            rectHotSpotLadinian.Right = 318;
            rectHotSpotLadinian.Bottom = 727;
            rectHotSpotLadinian.NavigateUrl = AttachJavaScriptForHotSpot("Ladinian (LA)");
            rectHotSpotLadinian.AlternateText = "Ladinian (LA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLadinian);

            RectangleHotSpot  rectHotSpotCarnian = new RectangleHotSpot();
            rectHotSpotCarnian.Left = 184;
            rectHotSpotCarnian.Top = 670;
            rectHotSpotCarnian.Right = 318;
            rectHotSpotCarnian.Bottom = 702;
            rectHotSpotCarnian.NavigateUrl = AttachJavaScriptForHotSpot("Carnian (CR)");
            rectHotSpotCarnian.AlternateText = "Carnian (CR)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCarnian);

            RectangleHotSpot  rectHotSpotNorian = new RectangleHotSpot();
            rectHotSpotNorian.Left = 184;
            rectHotSpotNorian.Top = 632;
            rectHotSpotNorian.Right = 317;
            rectHotSpotNorian.Bottom = 667;
            rectHotSpotNorian.NavigateUrl = AttachJavaScriptForHotSpot("Norian (NO)");
            rectHotSpotNorian.AlternateText = "Norian (NO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotNorian);

            RectangleHotSpot  rectHotSpotRhaetian = new RectangleHotSpot();
            rectHotSpotRhaetian.Left = 184;
            rectHotSpotRhaetian.Top = 621;
            rectHotSpotRhaetian.Right = 317;
            rectHotSpotRhaetian.Bottom = 632;
            rectHotSpotRhaetian.NavigateUrl = AttachJavaScriptForHotSpot("Rhaetian (RH)");
            rectHotSpotRhaetian.AlternateText = "Rhaetian (RH)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotRhaetian);


            RectangleHotSpot  rectHotSpotHettangian = new RectangleHotSpot();
            rectHotSpotHettangian.Left = 184;
            rectHotSpotHettangian.Top = 608;
            rectHotSpotHettangian.Right = 316;
            rectHotSpotHettangian.Bottom = 621;
            rectHotSpotHettangian.NavigateUrl = AttachJavaScriptForHotSpot("Hettangian (HE)");
            rectHotSpotHettangian.AlternateText = "Hettangian (HE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotHettangian);

            RectangleHotSpot  rectHotSpotSinemurian = new RectangleHotSpot();
            rectHotSpotSinemurian.Left = 184;
            rectHotSpotSinemurian.Top = 592;
            rectHotSpotSinemurian.Right = 317;
            rectHotSpotSinemurian.Bottom = 608;
            rectHotSpotSinemurian.NavigateUrl = AttachJavaScriptForHotSpot("Sinemurian (SM)");
            rectHotSpotSinemurian.AlternateText = "Sinemurian (SM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSinemurian);


            RectangleHotSpot  rectHotSpotPliensbachian = new RectangleHotSpot();
            rectHotSpotPliensbachian.Left = 184;
            rectHotSpotPliensbachian.Top = 573;
            rectHotSpotPliensbachian.Right = 317;
            rectHotSpotPliensbachian.Bottom = 592;
            rectHotSpotPliensbachian.NavigateUrl = AttachJavaScriptForHotSpot("Pliensbachian (PB)");
            rectHotSpotPliensbachian.AlternateText = "Pliensbachian (PB)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPliensbachian);

            RectangleHotSpot  rectHotSpotToarcian = new RectangleHotSpot();
            rectHotSpotToarcian.Left = 184;
            rectHotSpotToarcian.Top = 551;
            rectHotSpotToarcian.Right = 317;
            rectHotSpotToarcian.Bottom = 573;
            rectHotSpotToarcian.NavigateUrl = AttachJavaScriptForHotSpot("Toarcian (TC)");
            rectHotSpotToarcian.AlternateText = "Toarcian (TC)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotToarcian);

            RectangleHotSpot  rectHotSpotAalenian = new RectangleHotSpot();
            rectHotSpotAalenian.Left = 184;
            rectHotSpotAalenian.Top = 540;
            rectHotSpotAalenian.Right = 319;
            rectHotSpotAalenian.Bottom = 551;
            rectHotSpotAalenian.NavigateUrl = AttachJavaScriptForHotSpot("Aalenian (AA)");
            rectHotSpotAalenian.AlternateText = "Aalenian (AA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAalenian);

            RectangleHotSpot  rectHotSpotBajocian = new RectangleHotSpot();
            rectHotSpotBajocian.Left = 184;
            rectHotSpotBajocian.Top = 530;
            rectHotSpotBajocian.Right = 317;
            rectHotSpotBajocian.Bottom = 540;
            rectHotSpotBajocian.NavigateUrl = AttachJavaScriptForHotSpot("Bajocian (BJ)");
            rectHotSpotBajocian.AlternateText = "Bajocian (BJ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBajocian);

            RectangleHotSpot  rectHotSpotBathonian = new RectangleHotSpot();
            rectHotSpotBathonian.Left = 184;
            rectHotSpotBathonian.Top = 520;
            rectHotSpotBathonian.Right = 318;
            rectHotSpotBathonian.Bottom = 530;
            rectHotSpotBathonian.NavigateUrl = AttachJavaScriptForHotSpot("Bathonian (BT)");
            rectHotSpotBathonian.AlternateText = "Bathonian (BT)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBathonian);

            RectangleHotSpot  rectHotSpotCallovian = new RectangleHotSpot();
            rectHotSpotCallovian.Left = 184;
            rectHotSpotCallovian.Top = 509;
            rectHotSpotCallovian.Right = 318;
            rectHotSpotCallovian.Bottom = 520;
            rectHotSpotCallovian.NavigateUrl = AttachJavaScriptForHotSpot("Callovian (CN)");
            rectHotSpotCallovian.AlternateText = "Callovian (CN)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCallovian);

            RectangleHotSpot  rectHotSpotOxfordian = new RectangleHotSpot();
            rectHotSpotOxfordian.Left = 184;
            rectHotSpotOxfordian.Top = 493;
            rectHotSpotOxfordian.Right = 317;
            rectHotSpotOxfordian.Bottom = 509;
            rectHotSpotOxfordian.NavigateUrl = AttachJavaScriptForHotSpot("Oxfordian (OX)");
            rectHotSpotOxfordian.AlternateText = "Oxfordian (OX)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotOxfordian);

            RectangleHotSpot  rectHotSpotKimmeridgian = new RectangleHotSpot();
            rectHotSpotKimmeridgian.Left = 184;
            rectHotSpotKimmeridgian.Top = 478;
            rectHotSpotKimmeridgian.Right = 319;
            rectHotSpotKimmeridgian.Bottom = 493;
            rectHotSpotKimmeridgian.NavigateUrl = AttachJavaScriptForHotSpot("Kimmeridgian (KI)");
            rectHotSpotKimmeridgian.AlternateText = "Kimmeridgian (KI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotKimmeridgian);

            RectangleHotSpot  rectHotSpotTithonian = new RectangleHotSpot();
            rectHotSpotTithonian.Left = 184;
            rectHotSpotTithonian.Top = 465;
            rectHotSpotTithonian.Right = 318;
            rectHotSpotTithonian.Bottom = 478;
            rectHotSpotTithonian.NavigateUrl = AttachJavaScriptForHotSpot("Tithonian (TI)");
            rectHotSpotTithonian.AlternateText = "Tithonian (TI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTithonian);

            RectangleHotSpot  rectHotSpotLowerTriassic = new RectangleHotSpot();
            rectHotSpotLowerTriassic.Left = 95;
            rectHotSpotLowerTriassic.Top = 751;
            rectHotSpotLowerTriassic.Right = 184;
            rectHotSpotLowerTriassic.Bottom = 770;
            rectHotSpotLowerTriassic.NavigateUrl = AttachJavaScriptForHotSpot("Lower Triassic (RL)");
            rectHotSpotLowerTriassic.AlternateText = "Lower Triassic (RL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerTriassic);

            RectangleHotSpot  rectHotSpotMiddleTriassic = new RectangleHotSpot();
            rectHotSpotMiddleTriassic.Left = 95;
            rectHotSpotMiddleTriassic.Top = 703;
            rectHotSpotMiddleTriassic.Right = 184;
            rectHotSpotMiddleTriassic.Bottom = 751;
            rectHotSpotMiddleTriassic.NavigateUrl = AttachJavaScriptForHotSpot("Middle Triassic (RM)");
            rectHotSpotMiddleTriassic.AlternateText = "Middle Triassic (RM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiddleTriassic);

            RectangleHotSpot  rectHotSpotUpperTriassic = new RectangleHotSpot();
            rectHotSpotUpperTriassic.Left = 95;
            rectHotSpotUpperTriassic.Top = 621;
            rectHotSpotUpperTriassic.Right = 184;
            rectHotSpotUpperTriassic.Bottom = 703;
            rectHotSpotUpperTriassic.NavigateUrl = AttachJavaScriptForHotSpot("Upper Triassic (RU)");
            rectHotSpotUpperTriassic.AlternateText = "Upper Triassic (RU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperTriassic);

            RectangleHotSpot  rectHotSpotLowerJL = new RectangleHotSpot();
            rectHotSpotLowerJL.Left = 95;
            rectHotSpotLowerJL.Top = 551;
            rectHotSpotLowerJL.Right = 184;
            rectHotSpotLowerJL.Bottom = 621;
            rectHotSpotLowerJL.NavigateUrl = AttachJavaScriptForHotSpot("Lower (JL)");
            rectHotSpotLowerJL.AlternateText = "Lower (JL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerJL);

            RectangleHotSpot  rectHotSpotMiddleJM = new RectangleHotSpot();
            rectHotSpotMiddleJM.Left = 95;
            rectHotSpotMiddleJM.Top = 509;
            rectHotSpotMiddleJM.Right = 184;
            rectHotSpotMiddleJM.Bottom = 551;
            rectHotSpotMiddleJM.NavigateUrl = AttachJavaScriptForHotSpot("Middle (JM)");
            rectHotSpotMiddleJM.AlternateText = "Middle (JM)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiddleJM);


            RectangleHotSpot  rectHotSpotUpperJurassic = new RectangleHotSpot();
            rectHotSpotUpperJurassic.Left = 95;
            rectHotSpotUpperJurassic.Top = 465;
            rectHotSpotUpperJurassic.Right = 184;
            rectHotSpotUpperJurassic.Bottom = 509;
            rectHotSpotUpperJurassic.NavigateUrl = AttachJavaScriptForHotSpot("Upper Jurassic (JU)");
            rectHotSpotUpperJurassic.AlternateText = "Upper Jurassic (JU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperJurassic);

            RectangleHotSpot  rectHotSpotBerriasian = new RectangleHotSpot();
            rectHotSpotBerriasian.Left = 184;
            rectHotSpotBerriasian.Top = 450;
            rectHotSpotBerriasian.Right = 318;
            rectHotSpotBerriasian.Bottom = 464;
            rectHotSpotBerriasian.NavigateUrl = AttachJavaScriptForHotSpot("Berriasian (BE)");
            rectHotSpotBerriasian.AlternateText = "Berriasian (VA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBerriasian);


            RectangleHotSpot  rectHotSpotValenginian = new RectangleHotSpot();
            rectHotSpotValenginian.Left = 184;
            rectHotSpotValenginian.Top = 438;
            rectHotSpotValenginian.Right = 317;
            rectHotSpotValenginian.Bottom = 450;
            rectHotSpotValenginian.NavigateUrl = AttachJavaScriptForHotSpot("Valenginian (VA)");
            rectHotSpotValenginian.AlternateText = "Valenginian (VA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotValenginian);

            RectangleHotSpot  rectHotSpotHauterivian = new RectangleHotSpot();
            rectHotSpotHauterivian.Left = 184;
            rectHotSpotHauterivian.Top = 419;
            rectHotSpotHauterivian.Right = 319;
            rectHotSpotHauterivian.Bottom = 438;
            rectHotSpotHauterivian.NavigateUrl = AttachJavaScriptForHotSpot("Hauterivian (HT)");
            rectHotSpotHauterivian.AlternateText = "Hauterivian (HT)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotHauterivian);

            RectangleHotSpot  rectHotSpotBarremian = new RectangleHotSpot();
            rectHotSpotBarremian.Left = 184;
            rectHotSpotBarremian.Top = 406;
            rectHotSpotBarremian.Right = 316;
            rectHotSpotBarremian.Bottom = 419;
            rectHotSpotBarremian.NavigateUrl = AttachJavaScriptForHotSpot("Barremian (BR)");
            rectHotSpotBarremian.AlternateText = "Barremian (BR)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBarremian);

            RectangleHotSpot  rectHotSpotAptian = new RectangleHotSpot();
            rectHotSpotAptian.Left = 184;
            rectHotSpotAptian.Top = 369;
            rectHotSpotAptian.Right = 319;
            rectHotSpotAptian.Bottom = 406;
            rectHotSpotAptian.NavigateUrl = AttachJavaScriptForHotSpot("Aptian (AP)");
            rectHotSpotAptian.AlternateText = "Aptian (AP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAptian);

            RectangleHotSpot  rectHotSpotAlbian = new RectangleHotSpot();
            rectHotSpotAlbian.Left = 184;
            rectHotSpotAlbian.Top = 332;
            rectHotSpotAlbian.Right = 319;
            rectHotSpotAlbian.Bottom = 369;
            rectHotSpotAlbian.NavigateUrl = AttachJavaScriptForHotSpot("Albian (AB)");
            rectHotSpotAlbian.AlternateText = "Albian (AB)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotAlbian);

            RectangleHotSpot  rectHotSpotLowerCretaceous = new RectangleHotSpot();
            rectHotSpotLowerCretaceous.Left = 95;
            rectHotSpotLowerCretaceous.Top = 332;
            rectHotSpotLowerCretaceous.Right = 184;
            rectHotSpotLowerCretaceous.Bottom = 465;
            rectHotSpotLowerCretaceous.NavigateUrl = AttachJavaScriptForHotSpot("Lower Cretaceous (KL)");
            rectHotSpotLowerCretaceous.AlternateText = "Lower Cretaceous (KL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerCretaceous);

            RectangleHotSpot  rectHotSpotCenomanian = new RectangleHotSpot();
            rectHotSpotCenomanian.Left = 184;
            rectHotSpotCenomanian.Top = 315;
            rectHotSpotCenomanian.Right = 317;
            rectHotSpotCenomanian.Bottom = 332;
            rectHotSpotCenomanian.NavigateUrl = AttachJavaScriptForHotSpot("Cenomanian (CE)");
            rectHotSpotCenomanian.AlternateText = "Cenomanian (CE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCenomanian);

            RectangleHotSpot  rectHotSpotTuronian = new RectangleHotSpot();
            rectHotSpotTuronian.Left = 184;
            rectHotSpotTuronian.Top = 303;
            rectHotSpotTuronian.Right = 319;
            rectHotSpotTuronian.Bottom = 315;
            rectHotSpotTuronian.NavigateUrl = AttachJavaScriptForHotSpot("Turonian (TR)");
            rectHotSpotTuronian.AlternateText = "Turonian (TR)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTuronian);


            RectangleHotSpot  rectHotSpotConiacian = new RectangleHotSpot();
            rectHotSpotConiacian.Left = 184;
            rectHotSpotConiacian.Top = 292;
            rectHotSpotConiacian.Right = 317;
            rectHotSpotConiacian.Bottom = 303;
            rectHotSpotConiacian.NavigateUrl = AttachJavaScriptForHotSpot("Coniacian (CO)");
            rectHotSpotConiacian.AlternateText = "Coniacian (CO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotConiacian);

            RectangleHotSpot  rectHotSpotSantonian = new RectangleHotSpot();
            rectHotSpotSantonian.Left = 184;
            rectHotSpotSantonian.Top = 278;
            rectHotSpotSantonian.Right = 316;
            rectHotSpotSantonian.Bottom = 292;
            rectHotSpotSantonian.NavigateUrl = AttachJavaScriptForHotSpot("Santonian (SA)");
            rectHotSpotSantonian.AlternateText = "Santonian (SA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSantonian);


            RectangleHotSpot  rectHotSpotCampanian = new RectangleHotSpot();
            rectHotSpotCampanian.Left = 184;
            rectHotSpotCampanian.Top = 251;
            rectHotSpotCampanian.Right = 317;
            rectHotSpotCampanian.Bottom = 278;
            rectHotSpotCampanian.NavigateUrl = AttachJavaScriptForHotSpot("Campanian (CA)");
            rectHotSpotCampanian.AlternateText = "Campanian (CA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCampanian);

            RectangleHotSpot  rectHotSpotMaastrichtian = new RectangleHotSpot();
            rectHotSpotMaastrichtian.Left = 184;
            rectHotSpotMaastrichtian.Top = 236;
            rectHotSpotMaastrichtian.Right = 319;
            rectHotSpotMaastrichtian.Bottom = 251;
            rectHotSpotMaastrichtian.NavigateUrl = AttachJavaScriptForHotSpot("Maastrichtian (MA)");
            rectHotSpotMaastrichtian.AlternateText = "Maastrichtian (MA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMaastrichtian);

            RectangleHotSpot  rectHotSpotUpperCretaceous = new RectangleHotSpot();
            rectHotSpotUpperCretaceous.Left = 95;
            rectHotSpotUpperCretaceous.Top = 236;
            rectHotSpotUpperCretaceous.Right = 184;
            rectHotSpotUpperCretaceous.Bottom = 332;
            rectHotSpotUpperCretaceous.NavigateUrl = AttachJavaScriptForHotSpot("Upper Cretaceous (KU)");
            rectHotSpotUpperCretaceous.AlternateText = "Upper Cretaceous (KU)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperCretaceous);

            RectangleHotSpot  rectHotSpotDanian = new RectangleHotSpot();
            rectHotSpotDanian.Left = 171;
            rectHotSpotDanian.Top = 225;
            rectHotSpotDanian.Right = 317;
            rectHotSpotDanian.Bottom = 236;
            rectHotSpotDanian.NavigateUrl = AttachJavaScriptForHotSpot("Danian (DA)");
            rectHotSpotDanian.AlternateText = "Danian (DA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotDanian);

            RectangleHotSpot  rectHotSpotSelandian = new RectangleHotSpot();
            rectHotSpotSelandian.Left = 171;
            rectHotSpotSelandian.Top = 216;
            rectHotSpotSelandian.Right = 319;
            rectHotSpotSelandian.Bottom = 225;
            rectHotSpotSelandian.NavigateUrl = AttachJavaScriptForHotSpot("Selandian (SE)");
            rectHotSpotSelandian.AlternateText = "Selandian (SE)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotSelandian);

            RectangleHotSpot  rectHotSpotThanetian = new RectangleHotSpot();
            rectHotSpotThanetian.Left = 171;
            rectHotSpotThanetian.Top = 208;
            rectHotSpotThanetian.Right = 317;
            rectHotSpotThanetian.Bottom = 216;
            rectHotSpotThanetian.NavigateUrl =AttachJavaScriptForHotSpot("Thanetian (TA)");
            rectHotSpotThanetian.AlternateText = "Thanetian (TA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotThanetian);

            RectangleHotSpot  rectHotSpotYpresian = new RectangleHotSpot();
            rectHotSpotYpresian.Left = 171;
            rectHotSpotYpresian.Top = 188;
            rectHotSpotYpresian.Right = 317;
            rectHotSpotYpresian.Bottom = 207;
            rectHotSpotYpresian.NavigateUrl = AttachJavaScriptForHotSpot("Ypresian (YP)");
            rectHotSpotYpresian.AlternateText = "Ypresian (YP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotYpresian);

            RectangleHotSpot  rectHotSpotBartonian = new RectangleHotSpot();
            rectHotSpotBartonian.Left = 171;
            rectHotSpotBartonian.Top = 156;
            rectHotSpotBartonian.Right = 320;
            rectHotSpotBartonian.Bottom = 166;
            rectHotSpotBartonian.NavigateUrl = AttachJavaScriptForHotSpot("Bartonian (BA)");
            rectHotSpotBartonian.AlternateText = "Bartonian (BA)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotBartonian);


            RectangleHotSpot  rectHotSpotPriabonian = new RectangleHotSpot();
            rectHotSpotPriabonian.Left = 171;
            rectHotSpotPriabonian.Top = 147;
            rectHotSpotPriabonian.Right = 317;
            rectHotSpotPriabonian.Bottom = 156;
            rectHotSpotPriabonian.NavigateUrl = AttachJavaScriptForHotSpot("Priabonian (PO)");
            rectHotSpotPriabonian.AlternateText = "Priabonian (PO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPriabonian);

            RectangleHotSpot  rectHotSpotRupelian = new RectangleHotSpot();
            rectHotSpotRupelian.Left = 171;
            rectHotSpotRupelian.Top = 130;
            rectHotSpotRupelian.Right = 320;
            rectHotSpotRupelian.Bottom = 146;
            rectHotSpotRupelian.NavigateUrl = AttachJavaScriptForHotSpot("Rupelian (RP)");
            rectHotSpotRupelian.AlternateText = "Rupelian (RP)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotRupelian);


            RectangleHotSpot  rectHotSpotChattian = new RectangleHotSpot();
            rectHotSpotChattian.Left = 171;
            rectHotSpotChattian.Top = 113;
            rectHotSpotChattian.Right = 320;
            rectHotSpotChattian.Bottom = 129;
            rectHotSpotChattian.NavigateUrl = AttachJavaScriptForHotSpot("Chattian (CH)");
            rectHotSpotChattian.AlternateText = "Chattian (CH)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotChattian);

            RectangleHotSpot  rectHotSpotLowerMiocene = new RectangleHotSpot();
            rectHotSpotLowerMiocene.Left = 171;
            rectHotSpotLowerMiocene.Top = 94;
            rectHotSpotLowerMiocene.Right = 320;
            rectHotSpotLowerMiocene.Bottom = 112;
            rectHotSpotLowerMiocene.NavigateUrl = AttachJavaScriptForHotSpot("Lower Miocene (MI)");
            rectHotSpotLowerMiocene.AlternateText = "Lower Miocene (MI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotLowerMiocene);

            RectangleHotSpot  rectHotSpotMidMiocene = new RectangleHotSpot();
            rectHotSpotMidMiocene.Left = 171;
            rectHotSpotMidMiocene.Top = 80;
            rectHotSpotMidMiocene.Right = 320;
            rectHotSpotMidMiocene.Bottom = 94;
            rectHotSpotMidMiocene.NavigateUrl = AttachJavaScriptForHotSpot("Mid Miocene (MI)");
            rectHotSpotMidMiocene.AlternateText = "Mid Miocene (MI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMidMiocene);

            RectangleHotSpot  rectHotSpotUpperMiocene = new RectangleHotSpot();
            rectHotSpotUpperMiocene.Left = 171;
            rectHotSpotUpperMiocene.Top = 63;
            rectHotSpotUpperMiocene.Right = 320;
            rectHotSpotUpperMiocene.Bottom = 80;
            rectHotSpotUpperMiocene.NavigateUrl = AttachJavaScriptForHotSpot("Upper Miocene (MI)");
            rectHotSpotUpperMiocene.AlternateText = "Upper Miocene (MI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotUpperMiocene);

            RectangleHotSpot  rectHotSpotPaleocene = new RectangleHotSpot();
            rectHotSpotPaleocene.Left = 95;
            rectHotSpotPaleocene.Top = 208;
            rectHotSpotPaleocene.Right = 171;
            rectHotSpotPaleocene.Bottom = 236;
            rectHotSpotPaleocene.NavigateUrl = AttachJavaScriptForHotSpot("Paleocene (PC)");
            rectHotSpotPaleocene.AlternateText = "Paleocene (PC)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPaleocene);

            RectangleHotSpot  rectHotSpotEocene = new RectangleHotSpot();
            rectHotSpotEocene.Left = 95;
            rectHotSpotEocene.Top = 147;
            rectHotSpotEocene.Right = 170;
            rectHotSpotEocene.Bottom = 208;
            rectHotSpotEocene.NavigateUrl = AttachJavaScriptForHotSpot("Eocene (EO)");
            rectHotSpotEocene.AlternateText = "Eocene (EO)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotEocene);

            RectangleHotSpot  rectHotSpotOligocene = new RectangleHotSpot();
            rectHotSpotOligocene.Left = 95;
            rectHotSpotOligocene.Top = 113;
            rectHotSpotOligocene.Right = 171;
            rectHotSpotOligocene.Bottom = 147;
            rectHotSpotOligocene.NavigateUrl = AttachJavaScriptForHotSpot("Oligocene (OL)");
            rectHotSpotOligocene.AlternateText = "Oligocene (OL)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotOligocene);

            RectangleHotSpot  rectHotSpotPleistocene = new RectangleHotSpot();
            rectHotSpotPleistocene.Left = 95;
            rectHotSpotPleistocene.Top = 47;
            rectHotSpotPleistocene.Right = 320;
            rectHotSpotPleistocene.Bottom = 53;
            rectHotSpotPleistocene.NavigateUrl = AttachJavaScriptForHotSpot("Pleistocene (PS)");
            rectHotSpotPleistocene.AlternateText = "Pleistocene (PS)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPleistocene);

            RectangleHotSpot  rectHotSpotPliocene = new RectangleHotSpot();
            rectHotSpotPliocene.Left = 95;
            rectHotSpotPliocene.Top = 53;
            rectHotSpotPliocene.Right = 320;
            rectHotSpotPliocene.Bottom = 63;
            rectHotSpotPliocene.NavigateUrl = AttachJavaScriptForHotSpot("Pliocene (PI)");
            rectHotSpotPliocene.AlternateText = "Pliocene (PI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPliocene);

            RectangleHotSpot  rectHotSpotMiocene = new RectangleHotSpot();
            rectHotSpotMiocene.Left = 95;
            rectHotSpotMiocene.Top = 63;
            rectHotSpotMiocene.Right = 171;
            rectHotSpotMiocene.Bottom = 112;
            rectHotSpotMiocene.NavigateUrl = AttachJavaScriptForHotSpot("Miocene (MI)");
            rectHotSpotMiocene.AlternateText = "Miocene (MI)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMiocene);

            RectangleHotSpot  rectHotSpotTriassic = new RectangleHotSpot();
            rectHotSpotTriassic.Left = 52;
            rectHotSpotTriassic.Top = 621;
            rectHotSpotTriassic.Right = 95;
            rectHotSpotTriassic.Bottom = 770;
            rectHotSpotTriassic.NavigateUrl = AttachJavaScriptForHotSpot("Triassic (JJ)");
            rectHotSpotTriassic.AlternateText = "Triassic (JJ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotTriassic);

            RectangleHotSpot  rectHotSpotJurassic = new RectangleHotSpot();
            rectHotSpotJurassic.Left = 52;
            rectHotSpotJurassic.Top = 465;
            rectHotSpotJurassic.Right = 95;
            rectHotSpotJurassic.Bottom = 621;
            rectHotSpotJurassic.NavigateUrl = AttachJavaScriptForHotSpot("Jurassic (JJ)");
            rectHotSpotJurassic.AlternateText = "Jurassic (JJ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotJurassic);

            RectangleHotSpot  rectHotSpotCretaceous = new RectangleHotSpot();
            rectHotSpotCretaceous.Left = 52;
            rectHotSpotCretaceous.Top = 236;
            rectHotSpotCretaceous.Right = 95;
            rectHotSpotCretaceous.Bottom = 465;
            rectHotSpotCretaceous.NavigateUrl = AttachJavaScriptForHotSpot("Cretaceous (KK)");
            rectHotSpotCretaceous.AlternateText = "Cretaceous (KK)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCretaceous);


            RectangleHotSpot  rectHotSpotPaleogene = new RectangleHotSpot();
            rectHotSpotPaleogene.Left = 52;
            rectHotSpotPaleogene.Top = 113;
            rectHotSpotPaleogene.Right = 95;
            rectHotSpotPaleogene.Bottom = 236;
            rectHotSpotPaleogene.NavigateUrl = AttachJavaScriptForHotSpot("Paleogene (PG)");
            rectHotSpotPaleogene.AlternateText = "Paleogene (PG)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotPaleogene);

            RectangleHotSpot  rectHotSpotNeogene = new RectangleHotSpot();
            rectHotSpotNeogene.Left = 52;
            rectHotSpotNeogene.Top = 47;
            rectHotSpotNeogene.Right = 95;
            rectHotSpotNeogene.Bottom = 113;
            rectHotSpotNeogene.NavigateUrl = AttachJavaScriptForHotSpot("Neogene (NG)");
            rectHotSpotNeogene.AlternateText = "Neogene (NG)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotNeogene);


            RectangleHotSpot  rectHotSpotMesozoic = new RectangleHotSpot();
            rectHotSpotMesozoic.NavigateUrl = "chronstrat.aspx";
            rectHotSpotMesozoic.Left = 9;
            rectHotSpotMesozoic.Top = 236;
            rectHotSpotMesozoic.Right = 52;
            rectHotSpotMesozoic.Bottom = 771;
            rectHotSpotMesozoic.NavigateUrl = AttachJavaScriptForHotSpot("Mesozoic (MZ)");
            rectHotSpotMesozoic.AlternateText = "Mesozoic (MZ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotMesozoic);

            RectangleHotSpot  rectHotSpotCenozoic = new RectangleHotSpot();
            rectHotSpotCenozoic.Left = 9;
            rectHotSpotCenozoic.Top = 47;
            rectHotSpotCenozoic.Right = 52;
            rectHotSpotCenozoic.Bottom = 236;
            rectHotSpotCenozoic.NavigateUrl = AttachJavaScriptForHotSpot("Cenozoic (CZ)");
            rectHotSpotCenozoic.AlternateText = "Cenozoic (CZ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpotCenozoic);

            RectangleHotSpot  rectHotSpottest = new RectangleHotSpot();
            rectHotSpottest.Left = 577;
            rectHotSpottest.Top = 160;
            rectHotSpottest.Right = 592;
            rectHotSpottest.Bottom = 172;
            rectHotSpottest.NavigateUrl = AttachJavaScriptForHotSpot("test (CZ)");
            rectHotSpottest.AlternateText = "test (CZ)";
            imgMapChronostrat.HotSpots.Add(rectHotSpottest);


        }

        /// <summary>
        /// Attaches the java script for hot spot.
        /// </summary>
        /// <param name="hotSpotValue">The hot spot value.</param>
        /// <returns></returns>
        private string AttachJavaScriptForHotSpot(string hotSpotValue)
        {

            StringBuilder strHotSpotOnclickJavascript = new StringBuilder();
            strHotSpotOnclickJavascript.Append("javascript:HotSpotClick('");
            strHotSpotOnclickJavascript.Append(lblChrono.ClientID);
            strHotSpotOnclickJavascript.Append("','");
            strHotSpotOnclickJavascript.Append(hidFldChronostrat.ClientID);
            strHotSpotOnclickJavascript.Append("','");
            strHotSpotOnclickJavascript.Append(hotSpotValue);
            strHotSpotOnclickJavascript.Append("');");
           return strHotSpotOnclickJavascript.ToString();
        }

    }
}