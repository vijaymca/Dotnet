using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnzymeDAL;
using System.Data;
using EnzymeEntities.Entity;
using EnzymeErrorLog;


namespace EnzymeBAL
{
    public class clsDemographicBAL
    {


        clsDemographicDAL objAdminGBUSelectionDO = null;
        /// <summary>
        /// Used to get the keyword information.
        /// </summary>
        /// <returns>DataSet</returns>         
        public bool getCountrySitesDetails_BO(int inRegID, int inCntryID, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getCountrySitesDetails_DO(inRegID, inCntryID, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getCountryDetails_BAL(int regID, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getCountryDetailsDAL(regID, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getCompaignInfo_BAL(int compid, ref DataSet objDS)
        {

            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getCompaignInfoDAL(compid, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getEnzymeDetails_BAL(int demogrId, ref DataSet objDS)
        {
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getEnzymeDetailsDAL(demogrId, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getRegionsDetails_BAL(ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getRegionDetailsDAL(ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getSiteNameDetails_BAL(int regID, int cntryID, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getSiteNameDetailsDAL(regID, cntryID, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getDemographicDetails_BAL(ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getDemographicDetailsDAL(ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getBsnsCatSectr_BAL(int siteID, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getBsnsCatSectrDAL(siteID, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool getEnzymeDetails_BAL(ref DataSet objDS)
        {
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getEnzymeDetailsDAL(ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }
        //DemographicInfo
        public bool InsertInsertDemographicAndEnzyme_BAL(DemographicInfo demogrpahic, bool IsonlyCampaign, ref string strout, ref DataSet objDS)
        {
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (!IsonlyCampaign)
                {
                    if (objAdminGBUSelectionDO.InsertDemographicAndEnzymeDAL(demogrpahic,ref strout, ref objDS))
                        bReturn = true;
                    else
                    {
                        //Get the last error from DA class here.
                        //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                    }
                }
                else
                {
                    if (objAdminGBUSelectionDO.InsertCompaignDAL(demogrpahic,ref strout, ref objDS))
                        bReturn = true;
                    else
                    {
                        //Get the last error from DA class here.
                        //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                    }
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool UpdateDemographic_BAL(DemographicInfo demogrpahic, bool IsOther, ref string strout ,ref DataSet objDS)
        {
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.UpdateDemographicDAL(demogrpahic, IsOther,ref strout, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
                
        }

        public bool UpdateCompaign_BAL(DemographicInfo demogrpahic, ref string strout, ref DataSet objDS)
        {
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.UpdateCompaignDAL(demogrpahic,ref strout,  ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool DeteteCampaign_BAL(int ID, string ModifiedBy,ref string strout, ref DataSet objDS) 
            {
            
           bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.DeteteCampaignDAL(ID, ModifiedBy,ref strout, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
            }

        public bool getEnzynmeData_BAL(ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.getEnzynmeDataDAL(ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool UpdateEnzymeData_BAL(int Id, string Title, String CreatedBy, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.UpdateEnzymeDataDAL(Id, Title, CreatedBy, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool DeleteEnzymeData_BAL(int Id, string CreatedBy, int outputparam)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.DeleteEnzymeDataDAL(Id, CreatedBy, outputparam))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool InsertEnzymeData_BAL(string Title, String CreatedBy, int Outputparam)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.InsertEnzymeDataDAL(Title, CreatedBy, Outputparam))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        public bool DeteteDemographicDetails_BAL(int ID, string ModifiedBy, ref DataSet objDS)
        {
            //This will be returned back.
            bool bReturn = false;
            string m_strLastError;
            try
            {
                objAdminGBUSelectionDO = new clsDemographicDAL();
                //Call the data access function here

                if (objAdminGBUSelectionDO.DeteteDemographicDetailsDAL(ID, ModifiedBy, ref objDS))
                    bReturn = true;
                else
                {
                    //Get the last error from DA class here.
                    //m_strLastError = objAdminGBUSelectionDO.GetLastError();
                }
            }
            catch (Exception ex)
            {
                //Set the error to this variable
                m_strLastError = ex.StackTrace;
            }
            //Return the status here
            return bReturn;
        }

        //public bool getFiscalYr_BAL(ref DataSet objDS)
        //{
        //    bool bReturn = false;
        //    string m_strLastError;
        //    try
        //    {
        //        objAdminGBUSelectionDO = new clsDemographicDAL();
        //        //Call the data access function here

        //        if (objAdminGBUSelectionDO.getFiscalYrDAL(ref objDS))
        //            bReturn = true;
        //        else
        //        {
        //            //Get the last error from DA class here.
        //            //m_strLastError = objAdminGBUSelectionDO.GetLastError();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Set the error to this variable
        //        m_strLastError = ex.StackTrace;
        //    }
        //    //Return the status here
        //    return bReturn;
        //}

        public DataTable GetDropDownValuesBAL(MyLookup.Dropdowns drpName,ref bool flagStatus) {
            objAdminGBUSelectionDO = new clsDemographicDAL();
            return objAdminGBUSelectionDO.GetDropDownValuesDAL(drpName, ref flagStatus);
        }
    }
}
