#region " Imports "

using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using System.IO;
using System.IO.Compression;
using System.Drawing;

#endregion

namespace EnzymeWeb
{
    ////////////////////////////////////////////////////////////////////////////////////
    //
    //	File Description	: This file is used for Common Componenets which are using accross application.
    //                        
    // ---------------------------------------------------------------------------------
    //	Date Created		: MAY 14, 2013
    //	Author			    : SATISH KUMAR K
    // ---------------------------------------------------------------------------------
    // 	Change History
    //	Date Modified		: 
    //	Changed By		    :
    //	Change Description  : Disposing Objects, Comments and Format
    //
    ////////////////////////////////////////////////////////////////////////////////////

    public class clsUtils
    {
        #region "Variables"

        const string m_strModule = "clsUtils";


        #endregion

        #region "Methods"

        /// <summary>
        /// For Checking the Data Set has rows in specified numbered table
        /// </summary>
        /// <param name="ds">DataSet To be checked</param>
        /// <param name="intTableNumber">Table number to be checked - <b>Zero Based Index</b></param>
        /// <returns></returns>
        public static bool DataSetHasRows(DataSet ds, int intTableNumber)
        {
            if (ds == null) return false;
            else if (ds.Tables.Count == 0) return false;
            else if (ds.Tables.Count <= intTableNumber) return false;
            else if (ds.Tables[intTableNumber].Rows.Count == 0) return false;
            else return true;
        }

        /// <summary>
        /// For Checking the Data Table has rows or Not
        /// </summary>
        /// <param name="ds">Data Table To be checked</param>
        /// <returns></returns>
        public static bool DataTableHasRows(DataTable dt)
        {
            if (dt == null) return false;
            else if (dt.Rows.Count == 0) return false;
            else return true;
        }

        /// <summary>
        /// Dropdown set seleted value
        /// </summary>
        /// <param name="cmbBox"></param>
        /// <param name="value"></param>
        public static void SetDropdown(ASPxComboBox cmbBox, string value)
        {
            ListEditItem li = cmbBox.Items.FindByValue(value);
            if (li != null)
            {
                li.Selected = true;
            }
        }

        /// <summary>
        /// Binds the DataTable to ComboBox list with given value and text fields
        /// </summary>
        /// <param name="cmbBox">Name of the ComboBox list to process</param>
        /// <param name="dt">DataTable to Bind</param>
        /// <param name="strValue">Value field name as string</param>
        /// <param name="strText">Text fiels name as string</param>
        public static void BindDropDown(ASPxComboBox cmbBox, DataTable dt, string strValue, string strText)
        {
           // clsDataFunctionsBAL objFunctions = null;

            try
            {
                if (DataTableHasRows(dt))
                {
                    cmbBox.DataSource = dt;
                    cmbBox.ValueField = strValue;
                    cmbBox.TextField = strText;
                    cmbBox.DataBindItems();
                }
            }
            catch (Exception ex)
            {
                //objFunctions = new clsDataFunctionsBAL();
                //objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
               // objFunctions = null;
            }
        }

        /// <summary>
        /// For Binding the DevExpress RadioButton List
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="dt"></param>
        /// <param name="strValue"></param>
        /// <param name="strText"></param>
        public static void BindRadioButtonList(ASPxRadioButtonList rbl, DataTable dt, string strValue, string strText)
        {
            //clsDataFunctionsBAL objFunctions = null;

            try
            {
                if (DataTableHasRows(dt))
                {
                    rbl.DataSource = dt;
                    rbl.ValueField = strValue;
                    rbl.TextField = strText;
                    rbl.DataBind();
                }
            }
            catch (Exception ex)
            {
                //objFunctions = new clsDataFunctionsBAL();
                //objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
               // objFunctions = null;
            }
        }

        /// <summary>
        /// Binds the DataTable to Listbox with given value and text fields
        /// </summary>
        /// <param name="lstBox">Name of the Listbox to process</param>
        /// <param name="dt">DataTable to Bind</param>
        /// <param name="strValue">Value field name as string</param>
        /// <param name="strText">Text fiels name as string</param>
        public static void BindListBox(ASPxListBox lstBox, DataTable dt, string strValue, string strText)
        {
            //clsDataFunctionsBAL objFunctions = null;

            try
            {
                if (DataTableHasRows(dt))
                {
                    lstBox.DataSource = dt;
                    lstBox.ValueField = strValue;
                    lstBox.TextField = strText;
                    lstBox.DataBindItems();
                }
            }
            catch (Exception ex)
            {
                //objFunctions = new clsDataFunctionsBAL();
                //objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                //objFunctions = null;
            }
        }

        /// <summary>
        /// Gets the Values or Text of the Selected Items in the DropdownEdit
        /// </summary>
        /// <param name="DropdownEdit">DropdownEdit name in which we have to process</param>
        /// <param name="ListBoxName">Name of the Listbox within the Dropdown Edit</param>
        /// <param name="IsText">bool value to get Text or Value</param>
        /// <returns>Returns the Comma separated Value</returns>
        public static string GetDropDownEditSelectedData(ASPxDropDownEdit DropdownEdit, string ListBoxName, bool IsText)
        {
            StringBuilder fieldNames = new StringBuilder();
            ASPxListBox oOutputList = (ASPxListBox)DropdownEdit.FindControl(ListBoxName);
            if (oOutputList != null)
            {
                if (oOutputList.SelectedItems.Count > 0)
                {
                    foreach (ListEditItem ls in oOutputList.SelectedItems)
                    {
                        fieldNames.Append((IsText == true ? ls.Text.ToString() : ls.Value.ToString()) + ",");
                    }
                    fieldNames.Remove(fieldNames.Length - 1, 1);
                }
            }
            return fieldNames.ToString();
        }

        /// <summary>
        /// Gets the Values or Text of the Selected Items in the DropdownEdit
        /// </summary>
        /// <param name="DropdownEdit">DropdownEdit name in which we have to process</param>
        /// <param name="ListBoxName">Name of the Listbox within the Dropdown Edit</param>
        /// <param name="IsText">bool value to get Text or Value</param>
        /// <returns>Returns the Comma separated Value</returns>
        public static string GetListBoxSelectedData(ASPxListBox ListBoxName, bool IsText)
        {
            StringBuilder fieldNames = new StringBuilder();

            if (ListBoxName != null)
            {
                if (ListBoxName.SelectedItems.Count > 0)
                {
                    foreach (ListEditItem item in ListBoxName.SelectedItems)
                    {
                        fieldNames.Append((IsText == true ? item.Text.ToString() : item.Value.ToString()) + ",");
                    }
                    fieldNames.Remove(fieldNames.Length - 1, 1);
                }
            }
            return fieldNames.ToString();
        }

        /*
        /// <summary>
        /// Gets the selected value of DevExpress Combox as integer
        /// </summary>
        /// <param name="cmbBox">Name of the ComboBox to process</param>
        /// <returns>Value to Int</returns>
        public static int ConvertCmbValToInt(ASPxComboBox cmbBox)
        {
            clsDataFunctionsBAL objFunctions = null;

            try
            {
                return Convert.ToInt32(cmbBox.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                objFunctions = new clsDataFunctionsBAL();
                objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
                return 0;
            }
            finally
            {
                objFunctions = null;
            }
        }

        /// <summary>
        /// To select the comboBox Item
        /// </summary>
        /// <param name="cmbBox">Name of the ComboBox to process</param>
        /// <param name="objValue">Value to be selected</param>
        public static void SelectComboBoxItem(ASPxComboBox cmbBox, object objValue)
        {
            clsDataFunctionsBAL objFunctions = null;
            ListEditItem lstItem = null;

            try
            {
                lstItem = cmbBox.Items.FindByValue(clsUtils.strChkDBNull(objValue));
                if (lstItem != null) lstItem.Selected = true;
                else cmbBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                objFunctions = new clsDataFunctionsBAL();
                objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
                cmbBox.SelectedIndex = 0;
            }
            finally
            {
                objFunctions = null;
            }
        }

        /// <summary>
        /// For selecting the Radio Button List
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="objValue"></param>
        public static void SelectRadioButtonList(ASPxRadioButtonList rbl, object objValue)
        {
            clsDataFunctionsBAL objFunctions = null;
            ListEditItem lstItem = null;

            try
            {
                lstItem = rbl.Items.FindByValue(objValue);
                if (lstItem != null) lstItem.Selected = true;
            }
            catch (Exception ex)
            {
                objFunctions = new clsDataFunctionsBAL();
                objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
                rbl.SelectedIndex = 0;
            }
            finally
            {
                objFunctions = null;
            }
        }

        /// <summary>
        /// Selects the Listbox Values or Text inside the Dropdowneedit control
        /// for the given comma separated string
        /// </summary>
        /// <param name="lstBox">Name of the List box to process</param>
        /// <param name="strCommaSeparatedValues">Text/Value to be set</param>
        /// <param name="IsText">Text or Value</param>
        public static string SetListBoxSelection(ASPxListBox lstBox, string strCommaSeparatedValues, bool IsText)
        {
            string strSelectedText = "";
            string[] strArray = { };
            StringBuilder strSelected = null;
            clsDataFunctionsBAL objFunctions = null;

            try
            {
                if (strCommaSeparatedValues != "")
                {
                    strSelected = new StringBuilder();

                    //Clear the previous selection 
                    foreach (ListEditItem lei in lstBox.SelectedItems) strSelected.Append(lei.Value + ",");
                    if (strSelected.ToString().Length > 0)
                    {
                        strArray = (strSelected.ToString().TrimEnd(',')).Split(',');
                        foreach (string s in strArray) if (lstBox.Items.FindByValue(s) != null) lstBox.Items.FindByValue(s).Selected = false;
                    }

                    //Set new selection
                    strArray = strCommaSeparatedValues.Split(',');
                    if (IsText) foreach (string s in strArray) lstBox.Items.FindByText(s).Selected = true;
                    else foreach (string s in strArray) if (lstBox.Items.FindByValue(s) != null) lstBox.Items.FindByValue(s).Selected = true;

                    //Get selection text
                    foreach (ListEditItem lei in lstBox.SelectedItems)
                    {
                        strSelectedText = strSelectedText + ";" + lei.Text;
                    }
                    strSelectedText = strSelectedText.Trim(';');
                }
            }
            catch (Exception ex)
            {
                objFunctions = new clsDataFunctionsBAL();
                objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                strSelected = null;
                objFunctions = null;
            }

            return strSelectedText;
        }

        /// <summary>
        /// Checks whether the object (string) is DBNull or Empty        
        /// </summary>
        /// <param name="obj">Object to be converted to string</param>
        /// <returns></returns>
        public static string strChkDBNull(object obj)
        {
            clsDataFunctionsBAL objFunctions = null;

            try
            {
                if (object.ReferenceEquals(obj, DBNull.Value)) return string.Empty;
                else if (string.IsNullOrEmpty(Convert.ToString(obj))) return string.Empty;
                else return Convert.ToString(obj);
            }
            catch (Exception ex)
            {
                objFunctions = new clsDataFunctionsBAL();
                objFunctions.SaveErrorLog(ex.Message, m_strModule, MethodBase.GetCurrentMethod().Name);
                return string.Empty;
            }
            finally
            {
                objFunctions = null;
            }
        }

        /// <summary>
        /// Encrypts the string based on Encryption64 and Encodes the string based on UrlEncode
        /// </summary>
        /// <param name="strEncrDecr">String to be processed</param>
        /// <returns>Returns Encrypted string</returns>
        public static string Encrypt(string strEncrDecr)
        {
            Encryption64 objEncrypt = null;
            string strEncrDecrKey = string.Empty;

            try
            {
                objEncrypt = new Encryption64();

                strEncrDecrKey = ConfigurationManager.AppSettings["EncrDecrKey"].ToString();

                return objEncrypt.Encrypt(strEncrDecr, strEncrDecrKey);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                objEncrypt = null;
            }
        }

        /// <summary>
        /// Decodes the string based on UrlDecode and Decrypt the string based on Encryption64
        /// </summary>
        /// <param name="strEncrDecr">string to be processed</param>
        /// <returns>Returns Decrypted string</returns>
        public static string Decrypt(string strEncrDecr)
        {
            Encryption64 objEncrypt = null;
            string strEncrDecrKey = string.Empty;
            bool IsEncoded = true;
            string[] strArray = { };

            try
            {
                objEncrypt = new Encryption64();

                strEncrDecrKey = ConfigurationManager.AppSettings["EncrDecrKey"].ToString();

                strEncrDecr = strEncrDecr.Replace(' ', '+');
                strArray = new string[9] { "%3b", "%2f", "%23", "%3f", "%24", "%40", "%25", "%2b", "%3d" };
                foreach (string s in strArray) if (strEncrDecr.Contains(s)) IsEncoded = false;

                if (IsEncoded) return objEncrypt.Decrypt(strEncrDecr, strEncrDecrKey);
                else return objEncrypt.Decrypt(HttpContext.Current.Server.UrlDecode(strEncrDecr), strEncrDecrKey);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                objEncrypt = null;
            }
        }

        /// <summary>
        /// This function returns the next business working date discarding the weekends (saturday, sunday)
        /// by supplying
        ///      the starting date - from which date you want to know
        ///      the no. of days   - how many working days
        /// Useful in scenarios like from a date you want to calculate the need by date supplying working days
        /// Limitation: The function assumes that you are invoking this on a working day (not on a weekend) 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="nDays"></param>
        /// <returns></returns>
        public static DateTime GetNextWorkingDateByOffset(DateTime startDate, int nDays)
        {
            int workingDays = 0;
            int weekends = 0;

            DateTime currDate = startDate.AddDays(1);
            while (workingDays < nDays)
            {
                switch (currDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        {
                            weekends++;
                            break;
                        }
                    default:
                        {
                            workingDays++;
                            break;
                        }
                }
                currDate = currDate.AddDays(1);
            }
            return startDate.AddDays(nDays + weekends);
        }

        /// <summary>
        /// Construct old Mercury URL to Navigate
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string ConstructMercuryURL(string strRootFolderName, string strPath)
        {
            string[] strAppUriPath = { };
            string uri = HttpContext.Current.Request.Url.AbsoluteUri; //Get the application absolute URL path
            string retPath = string.Empty;
            string strHTTP = string.Empty;

            if (uri.ToString().Contains("https"))   //Check weather it was HTTP or HTTPS ..?  
            {
                uri = uri.Replace("https://", "");
                strHTTP = "https://";
            }
            else
            {
                uri = uri.Replace("http://", "");
                strHTTP = "http://";
            }

            strAppUriPath = uri.Split('/');
            strAppUriPath[0] = strHTTP + strAppUriPath[0];

            if (ConfigurationManager.AppSettings["URLFlag"] == "pg")
                retPath = string.Format("{0}/{1}/", strAppUriPath[0].ToString(), strRootFolderName);
            else
                retPath = string.Format("{0}/", strAppUriPath[0].ToString());

            return Path.Combine(retPath, strPath);
        }

        /// <summary>
        /// Check Sessions are exist or not.
        /// </summary>
        /// <param name="SessionKeys"></param>
        /// <param name="isPopup"></param>
        /// <returns></returns>
        public static string CheckSessionStatus(string[] SessionKeys, string[] QueryStrings, bool isPopup)
        {
            if (SessionKeys.Length == 0 && QueryStrings.Length == 0)
                return string.Empty;

            if (SessionKeys.Length > 0)
                foreach (string Key in SessionKeys)
                {
                    if (HttpContext.Current.Session[Key] == null || string.IsNullOrEmpty(HttpContext.Current.Session[Key].ToString()))
                    {
                        if (!isPopup)
                            return "<script type=\"text/javascript\" language=\"javascript\">alert(\"Required information is not supplied or Session expired. Page redirects to Country Selection page.\");window.location='" + clsUtils.ConstructMercuryURL(Convert.ToString(ConfigurationManager.AppSettings["MercuryRootFolderName"]), "LAeImport/CountrySelection.aspx") + "';</script>";
                        else
                            return "<script type=\"text/javascript\" language=\"javascript\">alert(\"Required information is not supplied or Session expired. Page redirects to Country Selection page.\");window.close();</script>";

                    }
                }

            if (QueryStrings.Length > 0)
                foreach (string Key in QueryStrings)
                {
                    if (HttpContext.Current.Request.QueryString[Key] == null || string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[Key].ToString()))
                    {
                        if (!isPopup)
                            return "<script type=\"text/javascript\" language=\"javascript\">alert(\"Required information is not supplied. Page redirects to Dashboard View page.\");window.location='DashboardView.aspx';</script>";
                        else
                            return "<script type=\"text/javascript\" language=\"javascript\">alert(\"Required information is not supplied. Page redirects to Dashboard View page.\");window.close();</script>";

                    }
                }

            return string.Empty;
        }

        /// <summary>
        /// This function deals with the compression of file bytes.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="strMessage"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(Stream input, ref string strMessage, ref bool status)
        {
            MemoryStream ms = null;
            GZipStream compressedzipStream = null;
            byte[] inputBytes = null;
            byte[] outputBytes = null;
            int count;
            long ReductionPerc = 0;
            try
            {
                inputBytes = new byte[input.Length];

                // Read the file to ensure it is readable.
                count = input.Read(inputBytes, 0, inputBytes.Length);
                if (count != inputBytes.Length)
                {
                    input.Close();
                    strMessage += string.Format("Please select the file or Unable to read data from file");
                    return null;
                }
                input.Close();

                ms = new MemoryStream();

                // Use the newly created memory stream for the compressed data.
                compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(inputBytes, 0, inputBytes.Length);

                if (ms.Length > inputBytes.Length)
                {
                    outputBytes = null;
                    status = false;
                    strMessage += string.Format("File already Compressed") + "</br>";
                }
                else
                {
                    //outputBytes = new byte[ms.Length];
                    //ms.Write(outputBytes, 0, outputBytes.Length);
                    outputBytes = ms.ToArray();
                    status = true;
                    ReductionPerc = ((inputBytes.Length - ms.Length) * 100 / inputBytes.Length);
                    strMessage += string.Format("Original size: {0}KB, Compressed size: {1}KB, Compression(%): {2}%", inputBytes.Length / 1024, ms.Length / 1024, ReductionPerc);
                }

            } // end try
            catch (InvalidDataException)
            {
                strMessage += string.Format("Error: The file being read contains invalid data.");
            }
            catch (FileNotFoundException)
            {
                strMessage += string.Format("Error:The file specified was not found.");
            }
            catch (ArgumentException)
            {
                strMessage += string.Format("Error: path is a zero-length string, contains only white space, or contains one or more invalid characters");
            }
            catch (PathTooLongException)
            {
                strMessage += string.Format("Error: The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
            }
            catch (DirectoryNotFoundException)
            {
                strMessage += string.Format("Error: The specified path is invalid, such as being on an unmapped drive.");
            }
            catch (IOException)
            {
                strMessage += string.Format("Error: An I/O error occurred while opening the file.");
            }
            catch (UnauthorizedAccessException)
            {
                strMessage += string.Format("Error: path specified a file that is read-only, the path is a directory, or caller does not have the required permissions.");
            }
            catch (IndexOutOfRangeException)
            {
                strMessage += string.Format("Error: You must provide parameters for MyGZIP.");
            }
            finally
            {
                // Close the stream.
                compressedzipStream.Close();
                ms.Close();
            }
            return outputBytes;

        }

        /// <summary>
        /// Decompress the compressed input bytes.
        /// </summary>
        /// <param name="compressedBytes"></param>
        /// <param name="length"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(byte[] compressedBytes, long length, ref string strMessage)
        {
            MemoryStream ms = null;
            GZipStream zipStream = null;
            byte[] decompressedBytes;
            int totalCount;
            try
            {
                // Reset the memory stream position to begin decompression.
                ms = new MemoryStream(compressedBytes);
                long mslen = ms.Length;
                //ms.Position = 0;
                zipStream = new GZipStream(ms, CompressionMode.Decompress);
                decompressedBytes = new byte[length + 100];

                // Use the ReadAllBytesFromStream to read the stream.
                totalCount = ReadAllBytesFromStream(zipStream, decompressedBytes);
                strMessage += string.Format(", Decompressed Size: {0}KB", totalCount / 1024) + "</br>";

            }
            finally
            {
                zipStream.Close();
                ms.Close();
            }

            return decompressedBytes;
        }

        /// <summary>
        /// Read all data from input stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            int totalCount = 0;
            int bytesRead = 0;
            while (true)
            {
                bytesRead = stream.Read(buffer, offset, 100);
                if (bytesRead == 0)
                {
                    bytesRead = stream.Read(buffer, offset, 100);
                    if (bytesRead == 0) break;
                }
                offset += bytesRead;
                totalCount += bytesRead;
            }
            return totalCount;
        }

        /// <summary>
        /// Removing Empty rows from the datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool RemoveEmptyRows(ref DataTable dt)
        {
            bool result = true;
            DataView dvFiltered = null;
            string query = string.Empty;

            try
            {
                dvFiltered = dt.DefaultView;

                foreach (DataColumn dc in dt.Columns)
                {
                    query += string.Format("({0} <>'' AND {0} IS NOT NULL) OR ", dc.ColumnName);
                }

                query = query.Remove(query.Length - 4, 4);

                dvFiltered.RowFilter = query;

                dt = dvFiltered.ToTable();
            }
            catch
            {
                result = false;
            }
            finally
            {
                dvFiltered = null;
            }

            return result;
        }

        /// <summary>
        /// Removing Empty rows from the datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool RemoveEmptyRows_New(ref DataTable dt)
        {
            bool result = true;
            DataTable dtFiltered = dt.Clone();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (!string.IsNullOrEmpty(dr[dc].ToString()))
                        {
                            dtFiltered.ImportRow(dr);
                            break;
                        }
                    }
                }

                dt = dtFiltered;
            }
            catch
            {
                result = false;
            }
            finally
            {
                dtFiltered = null;
            }

            return result;
        }

        /// <summary>
        /// Enables or Disables the Control and resets the control text or selection except Checkbox.
        /// </summary>
        /// <param name="divTables"></param>
        /// <param name="ControlName"></param>
        /// <param name="isDisable"></param>
        public static void DisableEnableCntrl(System.Web.UI.HtmlControls.HtmlGenericControl divTables, string ControlName, bool isDisable)
        {
            System.Web.UI.Control cntrl = divTables.FindControl(ControlName) as System.Web.UI.Control;
            System.Drawing.Color clrReadonly = (System.Drawing.Color)(System.Drawing.Color.LightGray);
            System.Drawing.Color clrReadonly_Fore = (System.Drawing.Color)(System.Drawing.Color.Black);

            switch (cntrl.GetType().Name)
            {
                case "ASPxTextBox":

                    ((ASPxTextBox)cntrl).ReadOnly = isDisable;
                    if (isDisable)
                    {
                        ((ASPxTextBox)cntrl).Text = "";
                        ((ASPxTextBox)cntrl).BackColor = clrReadonly;
                    }
                    else { ((ASPxTextBox)cntrl).BackColor = default(Color); }
                    break;
                case "ASPxComboBox":

                    ((ASPxComboBox)cntrl).Enabled = !isDisable;
                    if (isDisable)
                    {
                        ((ASPxComboBox)cntrl).BackColor = clrReadonly;
                        ((ASPxComboBox)cntrl).SelectedIndex = -1;
                    }
                    else { ((ASPxComboBox)cntrl).BackColor = default(Color); }
                    if (isDisable) ((ASPxComboBox)cntrl).ForeColor = clrReadonly_Fore;
                    break;
                case "ASPxDateEdit":

                    ((ASPxDateEdit)cntrl).Enabled = !isDisable;
                    if (isDisable)
                    {
                        ((ASPxDateEdit)cntrl).BackColor = clrReadonly;
                        ((ASPxDateEdit)cntrl).Text = "";
                    }
                    else { ((ASPxDateEdit)cntrl).BackColor = default(Color); }
                    break;

                case "ASPxDropDownEdit":
                    ((ASPxDropDownEdit)cntrl).Enabled = !isDisable;
                    if (isDisable)
                    { ((ASPxDropDownEdit)cntrl).BackColor = clrReadonly; }
                    else { ((ASPxDropDownEdit)cntrl).BackColor = default(Color); }
                    if (isDisable) ((ASPxDropDownEdit)cntrl).ForeColor = clrReadonly_Fore;
                    break;
                case "ASPxCheckBox":
                    ((ASPxCheckBox)cntrl).Enabled = !isDisable;
                    if (isDisable) ((ASPxCheckBox)cntrl).BackColor = clrReadonly;
                    break;
                case "ASPxRadioButtonList":

                    ((ASPxRadioButtonList)cntrl).Enabled = !isDisable;
                    ((ASPxRadioButtonList)cntrl).ReadOnly = isDisable;
                    if (isDisable)
                    {
                        ((ASPxRadioButtonList)cntrl).BackColor = clrReadonly;
                        ((ASPxRadioButtonList)cntrl).SelectedIndex = -1;
                    }
                    else { ((ASPxRadioButtonList)cntrl).BackColor = default(Color); }
                    break;
                case "ASPxCheckBoxList":

                    ((ASPxCheckBoxList)cntrl).Enabled = !isDisable;
                    if (isDisable)
                    {
                        ((ASPxCheckBoxList)cntrl).BackColor = clrReadonly;
                        ((ASPxCheckBoxList)cntrl).SelectedIndex = -1;
                    }
                    break;
                case "ASPxListBox":
                    ((ASPxListBox)cntrl).Enabled = !isDisable;
                    if (isDisable) ((ASPxListBox)cntrl).BackColor = clrReadonly;
                    break;
                case "ASPxMemo":

                    ((ASPxMemo)cntrl).Enabled = !isDisable;
                    if (isDisable)
                    {
                        ((ASPxMemo)cntrl).BackColor = clrReadonly;
                        ((ASPxMemo)cntrl).Text = "";
                    }
                    break;
                case "FileUpload":
                    ((FileUpload)cntrl).Enabled = !isDisable;
                    break;
                case "ImageButton":
                    ((ImageButton)cntrl).Enabled = !isDisable;
                    break;
                case "ASPxButton":
                    ((ASPxButton)cntrl).Enabled = !isDisable;
                    break;
            }
        }

        /// <summary>
        /// Show Alert Message
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="needLoading"></param>
        /// <param name="pathOrScript"></param>
        public static void ShowAlert(System.Web.UI.Page page, System.Type type, string key, string message, bool needLoading, string pathOrScript)
        {
            if (needLoading)
            {
                if (!string.IsNullOrEmpty(pathOrScript))
                {
                    page.ClientScript.RegisterStartupScript(type.GetType(), key, "alert('" + message.Replace("'","`") + "'); " + pathOrScript + "", true);
                    return;
                }

                page.ClientScript.RegisterStartupScript(type.GetType(), key, "alert('" + message.Replace("'","`") + "');window.location.href = window.location.href;", true);

            }
            else
            {
                page.ClientScript.RegisterStartupScript(type.GetType(), key, "alert('" + message.Replace("'","`") + "');", true);
            }
        }
        */
        #endregion
    }
}
