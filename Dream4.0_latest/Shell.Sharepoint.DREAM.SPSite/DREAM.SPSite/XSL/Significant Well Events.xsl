<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">


  <xsl:output method="html" indent ="yes"/>

  <!--Parameters-->
  <xsl:param name ="uwbi" select="'110000802651'"></xsl:param>
  <xsl:param name ="ChangedEvents" select="''"></xsl:param>
  <xsl:param name="recordCount" select="2"/>

  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <b>
      <xsl:value-of select="$recordCount" />
    </b>
    <xsl:choose >
      <xsl:when test ="number($recordCount)>1">
        results found.
      </xsl:when>
      <xsl:otherwise >
        result found.
      </xsl:otherwise>
    </xsl:choose>
    <br></br>
    <br></br>

    <div id="tableContainer" class="tableContainer">
      <!--table to display response data.-->
      <table style="border-right:1px solid #9B9797;" cellpadding="0" cellspacing="0" 
        id="tblSearchResults">
        <xsl:choose>
          <xsl:when test ="response/report/record">
            <xsl:for-each select="response/report/record[@recordno=1]">
              <thead class="fixedHeader" id="fixedHeader">
                <tr style="height: 20px;">
                  <th>
                    &#160;
                  </th>
                  <xsl:if test ="attribute[not(@recdetail)]">
                    <th>
                      Date&#160;&#160;<img id="rec_date_img_{generate-id()}" style="cursor:hand;" src="/_layouts/DREAM/Images/plus.gif" alt="Show Time" width="11px" height="11px" border="0" hspace="0">
                        <xsl:attribute name="onclick">
                          javascript:ShowDateWithTime(this);
                        </xsl:attribute>
                      </img >
                    </th>

                    <th>
                      Event Type
                    </th>

                    <th>
                      Pri
                    </th>

                    <th>
                      Owner
                    </th>

                    <th>
                      Remarks
                    </th>
                  </xsl:if >
                  <th>
                    Document
                  </th>
                </tr>
              </thead>
            </xsl:for-each>

            <!--Looping through all record having same report name.-->

            <tbody class="scrollContent" style="background-color:white">
              <xsl:for-each select="response/report/record">
                <xsl:variable name="eventidentifier">
                  <xsl:value-of select ="@recid"/>
                </xsl:variable>
                <xsl:variable name="datetypefilter">
                  <xsl:value-of select ="attribute[@name='DateType']/@value"/>
                </xsl:variable>
                <xsl:variable name="datewithtime">
                  <xsl:value-of select ="attribute[@name='DateTime']/@value"/>
                </xsl:variable>
                <xsl:variable name="datetimevalue">
                  <xsl:call-template name ="FormatDate">
                    <xsl:with-param name ="DateTime">
                      <xsl:value-of select ="attribute[@name='DateTime']/@value"/>
                    </xsl:with-param>
                    <xsl:with-param name ="withtime" select ="'true'"></xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <xsl:variable name ="liveLink">
                  <xsl:value-of select ="attribute[@name='Document']/@value"/>
                </xsl:variable>
                <xsl:variable name ="instancenumber">
                  <xsl:value-of select ="substring-after($liveLink,' ')"/>
                </xsl:variable>
                <xsl:variable name ="documentnumber">
                  <xsl:value-of select ="substring-before($liveLink,' ')"/>
                </xsl:variable>
                <xsl:variable name ="eventdate">
                  <xsl:value-of select ="attribute[@name='Date']/@value"/>
                </xsl:variable>
                <xsl:variable name ="eventtype">
                  <xsl:value-of select ="attribute[@name='Code']/@value"/>
                </xsl:variable>
                <xsl:variable name ="eventowner">
                  <xsl:value-of select ="attribute[@name='Owner']/@value"/>
                </xsl:variable>
                <xsl:variable name ="typeandcode">
                  <xsl:value-of select ="attribute[@name='Type']/@value"/> - <xsl:value-of select ="attribute[@name='Code']/@value"/>
                </xsl:variable>
                <xsl:if test="position()  mod 2 = 1">
                  <tr>
                    <xsl:attribute name ="style">
                      background-color:
                      <xsl:call-template name ="HighlightRow">
                        <xsl:with-param name ="eventidparam" select ="$eventidentifier"></xsl:with-param>
                        <xsl:with-param name ="changedeventsparam" select ="$ChangedEvents"></xsl:with-param>
                        <xsl:with-param name ="alternaterow" select ="'false'"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:attribute>
                    <td style="word-wrap:break-word;vertical-align:top" width="50">
                      <img id="rec_img_{generate-id()}" style="cursor:hand;" onclick="HideExpandDataOwnerMenu('{generate-id()}')" src="/_layouts/DREAM/Images/plus.gif" alt="Show Detail" width="11px" height="11px" border="0" hspace="0"/>
                    </td>
                    <a>
                      <xsl:attribute name="name">
                        <xsl:value-of select="@recid"></xsl:value-of>
                      </xsl:attribute>
                      <xsl:if test ="attribute[not(@recdetail)]">
                        <!--Date column-->
                        <td style="vertical-align:top;text-align:center">
                          <div id="valuewithtime" style="display:none">
                            <xsl:choose>
                              <xsl:when test="string($datetimevalue)">
                                <xsl:value-of select="$datetimevalue" />
                              </xsl:when >
                              <xsl:otherwise >
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </div >
                          <div id="valuewithouttime" style="display:block">
                            <xsl:call-template name ="FormatDate" >
                              <xsl:with-param name ="DateTime" select ="attribute[@name='DateTime']/@value"></xsl:with-param>
                              <xsl:with-param name ="withtime" select ="'false'"></xsl:with-param>
                            </xsl:call-template>
                          </div >
                        </td>
                        <!--end of Date Column-->
                        <td style="word-wrap:break-word;vertical-align:top"  width="150">
                          <xsl:choose>
                            <xsl:when test="string($typeandcode)">
                              <xsl:value-of select ="$typeandcode"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;text-align:center"  width="50">
                          <xsl:choose>
                            <xsl:when test="string(attribute[@name='Priority']/@value)">
                              <xsl:value-of select ="attribute[@name='Priority']/@value"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top"  width="150">
                          <xsl:choose>
                            <xsl:when test="string(attribute[@name='Owner']/@value)">
                              <xsl:choose>
                                <xsl:when test ="contains(attribute[@name='Owner']/@value,'EDM Event') or contains(attribute[@name='Owner']/@value,'EDM Activity') or contains(attribute[@name='Owner']/@value,'EDM Daily')">
                                  <a href="javascript:void(0)">
                                    <xsl:attribute name="onclick">
                                      javascript:ViewEdmInfo('<xsl:value-of select="$eventidentifier"/>');
                                    </xsl:attribute>
                                    <xsl:value-of select ="attribute[@name='Owner']/@value"/>
                                  </a >
                                </xsl:when>
                                <xsl:otherwise >
                                  <xsl:value-of select ="attribute[@name='Owner']/@value"/>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;" width="450">
                          <xsl:if test="string(attribute[@name='Remarks']/@value)">
                            <xsl:value-of  select="attribute[@name='Remarks']/@value" />
                          </xsl:if >
                          &#160;
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;text-align:center">
                          <xsl:choose >
                            <xsl:when test="string($liveLink) and string($documentnumber)">
                              <a href="javascript:void(0)">
                                <xsl:attribute name="onclick">
                                  javascript:OpenLiveLink('<xsl:value-of select ="$instancenumber"/>','<xsl:value-of select ="$documentnumber"/>');
                                </xsl:attribute>
                                <xsl:value-of select ="$documentnumber"/>
                              </a>
                            </xsl:when >
                            <xsl:otherwise >
                             &#160;
                            </xsl:otherwise>
                          </xsl:choose>
                        </td>
                      </xsl:if >
                    </a>
                  </tr>
                </xsl:if >
                <xsl:if test="position()  mod 2 = 0">
                  <tr>
                    <xsl:attribute name ="style">
                      background-color:
                      <xsl:call-template name ="HighlightRow">
                        <xsl:with-param name ="eventidparam" select ="$eventidentifier"></xsl:with-param>
                        <xsl:with-param name ="changedeventsparam" select ="$ChangedEvents"></xsl:with-param>
                        <xsl:with-param name ="alternaterow" select ="'true'"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:attribute>
                    <td style="word-wrap:break-word;vertical-align:top" width="50">
                      <img id="rec_img_{generate-id()}" style="cursor:hand;" onclick="HideExpandDataOwnerMenu('{generate-id()}')" src="/_layouts/DREAM/Images/plus.gif" alt="Show Detail" width="11px" height="11px" border="0" hspace="0"/>
                    </td>
                    <a>
                      <xsl:attribute name="name">
                        <xsl:value-of select="@recid"></xsl:value-of>
                      </xsl:attribute>
                      <xsl:if test ="attribute[not(@recdetail)]">
                        <!--Date column-->
                        <td style="vertical-align:top;text-align:center">
                          <div id="valuewithtime" style="display:none">
                            <xsl:choose>
                              <xsl:when test="string($datetimevalue)">
                                <xsl:value-of select="$datetimevalue" />
                              </xsl:when >
                              <xsl:otherwise >
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </div >
                          <div id="valuewithouttime" style="display:block">
                            <xsl:call-template name ="FormatDate" >
                              <xsl:with-param name ="DateTime" select ="attribute[@name='DateTime']/@value"></xsl:with-param>
                              <xsl:with-param name ="withtime" select ="'false'"></xsl:with-param>
                            </xsl:call-template>
                          </div >
                        </td>
                        <!--end of Date Column-->
                        <td style="word-wrap:break-word;vertical-align:top"  width="150">
                          <xsl:choose>
                            <xsl:when test="string($typeandcode)">
                              <xsl:value-of select ="$typeandcode"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;text-align:center"  width="50">
                          <xsl:choose>
                            <xsl:when test="string(attribute[@name='Priority']/@value)">
                              <xsl:value-of select ="attribute[@name='Priority']/@value"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top"  width="150">
                          <xsl:choose>
                            <xsl:when test="string(attribute[@name='Owner']/@value)">
                              <xsl:choose>
                                <xsl:when test ="contains(attribute[@name='Owner']/@value,'EDM Event') or contains(attribute[@name='Owner']/@value,'EDM Activity') or contains(attribute[@name='Owner']/@value,'EDM Daily')">
                                  <a href="javascript:void(0)">
                                    <xsl:attribute name="onclick">
                                      javascript:ViewEdmInfo('<xsl:value-of select="$eventidentifier"/>');
                                    </xsl:attribute>
                                    <xsl:value-of select ="attribute[@name='Owner']/@value"/>
                                  </a >
                                </xsl:when>
                                <xsl:otherwise >
                                  <xsl:value-of select ="attribute[@name='Owner']/@value"/>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;" width="450">
                          <xsl:if test="string(attribute[@name='Remarks']/@value)">
                            <xsl:value-of  select="attribute[@name='Remarks']/@value" />
                          </xsl:if >
                          &#160;
                        </td>
                        <td style="word-wrap:break-word;vertical-align:top;text-align:center">
                          <xsl:choose >
                            <xsl:when test="string($liveLink) and string($documentnumber)">
                              <a href="javascript:void(0)">
                                <xsl:attribute name="onclick">
                                  javascript:OpenLiveLink('<xsl:value-of select ="$instancenumber"/>','<xsl:value-of select ="$documentnumber"/>');
                                </xsl:attribute>
                                <xsl:value-of select ="$documentnumber"/>
                              </a>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose>
                        </td>
                      </xsl:if>
                    </a>
                  </tr>
                </xsl:if >
                <xsl:if test="attribute[@recdetail='true']">
                  <tr>
                    <td colspan="9" style="border-right:none">
                      <div id="detailDiv_{generate-id()}" onclick="HideExpandDataOwnerMenu('{generate-id()}')" style="cursor:hand;font-weight:bold;display:none;" >
                        <table>
                          <tr>
                            <td bgcolor="#EFEFEF">
                              ID
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='EventId']/@value)">
                                  <xsl:value-of select="attribute[@name='EventId']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>
                          <tr>
                            <td bgcolor="#EFEFEF">
                              Created
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='CreatedOn']/@value)">
                                  <xsl:call-template name ="FormatDate" >
                                    <xsl:with-param name ="DateTime" select ="attribute[@name='CreatedOn']/@value"></xsl:with-param>
                                    <xsl:with-param name ="withtime" select ="'true'"></xsl:with-param>
                                  </xsl:call-template>
                                  &#160; <xsl:value-of select="attribute[@name='CreatedBy']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>
                          <tr>
                            <td bgcolor="#EFEFEF">
                              Updated
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='UpdatedOn']/@value)">
                                  <xsl:call-template name ="FormatDate" >
                                    <xsl:with-param name ="DateTime" select ="attribute[@name='UpdatedOn']/@value"></xsl:with-param>
                                    <xsl:with-param name ="withtime" select ="'true'"></xsl:with-param>
                                  </xsl:call-template>
                                  &#160; <xsl:value-of select="attribute[@name='UpdatedBy']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>
                          <tr>
                            <td bgcolor="#EFEFEF">
                              LinkInfo
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='LinkInfo']/@value)">
                                  <xsl:value-of select="attribute[@name='LinkInfo']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>
                          <tr>
                            <td bgcolor="#EFEFEF">
                              Level
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='Level']/@value)">
                                  <xsl:value-of select="attribute[@name='Level']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>

                          <tr>
                            <td bgcolor="#EFEFEF">
                              Date Type
                            </td>
                            <td style="word-wrap:break-word;vertical-align:top"  width="600">
                              <xsl:choose >
                                <xsl:when test="string(attribute[@name='DateType']/@value)">
                                  <xsl:value-of select="attribute[@name='DateType']/@value" />
                                </xsl:when >
                                <xsl:otherwise >
                                  &#160;
                                </xsl:otherwise>
                              </xsl:choose >
                            </td>
                          </tr>

                        </table>

                      </div>
                    </td>
                  </tr >
                </xsl:if>
              </xsl:for-each >
            </tbody>
          </xsl:when>
          <xsl:otherwise>
            <tr>
              <td colspan="9" class="tblWellEvent">There are no records to display</td>
            </tr>
          </xsl:otherwise>
        </xsl:choose>
      </table>
      <!--End of table-->
      <Script language="javascript">
        FixColWidth("tblSearchResults");
        CarriageReturnHandling('Remarks ','tblSearchResults');
      </Script>
    </div>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <xsl:param name="withtime" />
    <!-- new date format 2006-01-14T08:55:22 -->
    <xsl:variable name="year">
      <xsl:value-of select="substring($DateTime,3,2)" />
    </xsl:variable>
    <xsl:variable name="month">
      <xsl:value-of select="substring($DateTime,6,2)" />
    </xsl:variable>
    <xsl:variable name="day">
      <xsl:value-of select="substring($DateTime,9,2)" />
    </xsl:variable>
    <xsl:variable name ="DatePart">
      <xsl:value-of select="$year" />
      <xsl:value-of select="'-'"/>
      <xsl:value-of select="$month" />
      <xsl:value-of select="'-'"/>
      <xsl:value-of select="$day" />
    </xsl:variable>
    <xsl:variable name="time">
      <xsl:value-of select="substring-after($DateTime,$DatePart)" />
    </xsl:variable>
    <xsl:value-of select="$day"/>
    <xsl:value-of select="'-'"/>
    <xsl:choose>
      <xsl:when test="$month = '01'">Jan</xsl:when>
      <xsl:when test="$month = '02'">Feb</xsl:when>
      <xsl:when test="$month = '03'">Mar</xsl:when>
      <xsl:when test="$month = '04'">Apr</xsl:when>
      <xsl:when test="$month = '05'">May</xsl:when>
      <xsl:when test="$month = '06'">Jun</xsl:when>
      <xsl:when test="$month = '07'">Jul</xsl:when>
      <xsl:when test="$month = '08'">Aug</xsl:when>
      <xsl:when test="$month = '09'">Sep</xsl:when>
      <xsl:when test="$month = '10'">Oct</xsl:when>
      <xsl:when test="$month = '11'">Nov</xsl:when>
      <xsl:when test="$month = '12'">Dec</xsl:when>
    </xsl:choose>
    <xsl:value-of select="'-'"/>
    <xsl:value-of select="$year"/>
    <xsl:if test ="$withtime='true'">
      <xsl:value-of select ="' '"/>
      <xsl:value-of select="substring($time,2,5)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name ="HighlightRow">
    <xsl:param name="eventidparam" />
    <xsl:param name="changedeventsparam" />
    <xsl:param name="alternaterow" />

    <xsl:variable name ="eventidchecker">
      <xsl:value-of select ="'F'"/>
      <xsl:value-of select ="$eventidparam"/>
      <xsl:value-of select ="'F'"/>
    </xsl:variable>
    <xsl:variable name ="rowcolor">
      <xsl:choose >
        <xsl:when test="contains($changedeventsparam,$eventidchecker)">
          #CCFFFF
        </xsl:when>
        <xsl:otherwise >
          <xsl:choose >
            <xsl:when test ="$alternaterow='true'">
              #EFEFEF
            </xsl:when>
            <xsl:otherwise >
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select ="$rowcolor"/>
  </xsl:template>
</xsl:stylesheet >
