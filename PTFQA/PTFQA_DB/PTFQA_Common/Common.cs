using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace PTFQA_Common
{
 public class Common
    {
        #region "Methods"

        /// <summary>
        /// Get Application Error Log Path
        /// </summary>
        /// <returns>string</returns>
        public string GetLogPath()
        {
            string strFolderPath = string.Empty, strFileNameFormat = string.Empty, strFilePath = string.Empty;

            try
            {
                strFileNameFormat = string.Format("{0}-{1}-{2}",
                                  Convert.ToString(DateTime.Now.Date.Month),
                                  Convert.ToString(DateTime.Now.Date.Day),
                                  Convert.ToString(DateTime.Now.Date.Year));

                strFolderPath = HttpContext.Current.Server.MapPath("~\\ErrorLog");


                if (!Directory.Exists(strFolderPath))
                    Directory.CreateDirectory(strFolderPath);

                strFilePath = string.Format("{0}/{1}.txt", strFolderPath, strFileNameFormat);
            }
            catch
            {
                strFilePath = string.Empty;
                throw;
            }

            return strFilePath;
        }

        /// <summary>
        /// Write Log details to File
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strFileData"></param>
        /// <param name="bAppend"></param>
        /// <returns>bool</returns>
        public bool WriteToFile(string strFilePath, string strFileData, bool bAppend)
        {
            bool bReturn = false;
            StreamWriter objSWriter = null;
            FileInfo objFile = null;

            try
            {
                objSWriter = default(StreamWriter);
                objFile = new FileInfo(strFilePath);

                if (!objFile.Exists)
                    objFile.Create();

                objSWriter = new StreamWriter(strFilePath, bAppend);
                objSWriter.WriteLine(System.Environment.NewLine + strFileData);

                bReturn = true;
            }
            catch
            {
                bReturn = false;
                throw;
            }
            finally
            {
                if (objSWriter != null)
                {
                    objSWriter.Close();
                    objSWriter.Dispose();
                }
                objSWriter = null;
                objFile = null;
            }

            return bReturn;
        }

        /// <summary>
        /// Formating the US Phone Number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>string</returns>
        public string GetUsPhoneFormat(string phoneNumber)
        {
            string result = string.Empty;

            try
            {
                switch (phoneNumber.Length)
                {
                    case 7:
                        result = string.Format("{0}-{1}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 4));
                        break;
                    case 10:
                        result = string.Format("({0}){1}-{2}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3),
                                               phoneNumber.Substring(6, 4));
                        break;
                    case 15:
                        result = string.Format("({0}){1}-{2}", phoneNumber.Substring(3, 3), phoneNumber.Substring(7, 3),
                                               phoneNumber.Substring(11, 4));
                        break;
                    default:
                        result = phoneNumber;
                        break;
                }
            }

            catch
            {
                result = string.Empty;
                throw;
            }

            return result;
        }

        /// <summary>
        /// To delete files in specific folder
        /// </summary>
        public void DeleteFiles(string folderPath)
        {
            string[] fileList = { };

            try
            {
                if (Directory.Exists(folderPath))
                {
                    fileList = Directory.GetFiles(folderPath);

                    foreach (string strFile in fileList)
                    {
                        //To delete a file from directory
                        if (File.Exists(strFile))
                            File.Delete(strFile);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
