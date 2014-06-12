<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>

  <xsl:param name="windowTitle" select="''"/>
  <xsl:variable name="eventHdrLnth" select="0"/>
  <xsl:variable name="dailyHdrLnth" select="0"/>
  <xsl:variable name="actHdrLnth" select="0"/>
  
  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <div id="tabs">
    <div class="gray_embossed_tabs_r" >
      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
          <td>
            <ul class="tabNavigation">
              <li>
                <a>
                  <span style="font-weight:bold;color:black">Events</span>
                  </a>
              </li>
            </ul>
          </td>
        </tr>
        <tr>
          <td class="underline">&#160;&#160;&#160;&#160;</td>
        </tr>
      </table>
    </div>
    <div id="tableContainer" class="tableContainer">
      <!--table to display response data.-->
      <table cellpadding="0" cellspacing="0" 
        class="scrollTable EDMTblBorder" id="tblSearchResults" style="border-right:10px solid #bdbdbd;border-bottom:5px solid #bdbdbd;">
        <!--Looping through all record having same report name.-->
        <tbody class="scrollContent">
          <xsl:for-each select="response/report">
            <tr >
              <td style="width:auto;">
                <img src="/_LAYOUTS/DREAM/images/plus.gif" alt="open" width="11" height="11" border="0" hspace="0" onclick="ExpandCollapse(this);" />
              </td>
              <td style="width:auto;">
                <b> <xsl:value-of select="event[1]/record/attribute[@name='Well']/@value"></xsl:value-of></b>
              </td>
            </tr>
            <tr style="display:none">
             <td style="width:auto;">
                &#160;
              </td>
            <td style="width:auto;padding:10px 10px 10px 10px;">
                <b>Events</b>
                <table id="eventtbl" cellpadding="0" cellspacing="0" class="EDMTblBorder" style="border-right:2px solid #bdbdbd;" width="95%">
                 
                    <tr>
                    <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">&#160;</th>
                    <xsl:for-each select="event[1]/record/attribute">
                      <xsl:if test="@display!='false'">
                        <xsl:variable name="eventHdrLnth" select="count(attribute)" />
                        <th style="text-align:center;width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
                         <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                    </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </th>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tbody class="scrollContent" >
                  <xsl:for-each select="event">
                    <tr >
                      <xsl:choose>
                        <xsl:when test ="count(daily)&gt;0">
                          <td style="width:auto;">
                            <img src="/_LAYOUTS/DREAM/images/plus.gif" alt="open" width="11" height="11" border="0" hspace="0" onclick="ExpandCollapse(this);" />
                          </td>
                        </xsl:when>
                        <xsl:otherwise>
                          <td style="width:auto;">&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>

                      
                      <xsl:for-each select="record/attribute">
                        <xsl:if test="@display!='false'">
                          <xsl:choose>
                            <xsl:when test="string-length(string(@value))>0">
                               <xsl:choose>                                                 		
                            		<xsl:when test="@type = 'number'">
                                          <td style="text-align:right;width:auto">
                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                          </td>
									</xsl:when>
                            		<xsl:otherwise>
                            			<td style="width:auto">
                                		<xsl:value-of select="@value"/>
                              			</td>
                            		</xsl:otherwise>
   								</xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                              <td style="width:auto;">&#160;</td>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:if>
                      </xsl:for-each>
                    </tr>
                    <xsl:if test ="count(daily)&gt;0">
                      <tr style="display:none">
                        <td style="width:auto;">
                          &#160;
                        </td>
                        <td style="width:auto;padding:10px 10px 10px 10px;">
                          <xsl:attribute name="colspan">
                            <xsl:value-of select="count(record/attribute[@display!='false'])"/>
                          </xsl:attribute>
                          <b>Daily Summary</b>
                          <table id="dailySummarytbl" cellpadding="0" cellspacing="0" class="EDMTblBorder" style=" border-right:2px solid #bdbdbd;" width="95%">

                            <tr>
                              <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">&#160;</th>
                              <xsl:for-each select="daily[1]/record/attribute">
                                <xsl:if test="@display!='false'">
                                  <th style="text-align:center;width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
                                   <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                    </xsl:call-template>
                                    <xsl:value-of select="@name"/>
                                  </th>
                                </xsl:if>
                              </xsl:for-each>
                            </tr>

                            <tbody class="scrollContent" >
                              <xsl:for-each select="daily">
                                <tr >
                                  <xsl:choose>
                                    <xsl:when test ="count(activity/record)&gt;0">
                                      <td style="width:auto;">
                                        <img src="/_LAYOUTS/DREAM/images/plus.gif" alt="open" width="11" height="11" border="0" hspace="0" onclick="ExpandCollapse(this);" />
                                      </td>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <td style="width:auto;">&#160;</td>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <xsl:for-each select="record/attribute">
                                    <xsl:if test="@display!='false'">
                                      <xsl:choose>
                                        <xsl:when test="string-length(string(@value))>0">
                                          <xsl:choose>                                                 		
                            		<xsl:when test="@type = 'number'">
                                          <td style="text-align:right;width:auto">
                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                          </td>
									</xsl:when>
                            		<xsl:otherwise>
                            			<td style="width:auto">
                                		<xsl:value-of select="@value"/>
                              			</td>
                            		</xsl:otherwise>
   								</xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>
                                          <td>&#160;</td>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:if>
                                  </xsl:for-each>
                                </tr>
                                <xsl:if test ="count(activity/record)&gt;0">
                                  <tr style="display:none">
                                    <td style="width:auto;">
                                      &#160;
                                    </td>
                                    <td style="width:auto;padding:10px 10px 10px 10px;" >
                                      <xsl:attribute name="colspan">
                                        <xsl:value-of select="count(record/attribute[@display!='false'])"/>
                                      </xsl:attribute>
                                      <b>Reported Activities</b>
                                      <table id="dailyActivitytbl" cellpadding="0" cellspacing="0" class="EDMTblBorder" style="border-right:2px solid #bdbdbd;">

                                        <tr>
                                          <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">&#160;</th>
                                          <xsl:for-each select="activity[1]/record[1]/attribute">
                                            <xsl:if test="@display!='false'">
                                              <th style="text-align:center;width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
                                               <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                    </xsl:call-template>
                                                <xsl:value-of select="@name"/>
                                              </th>
                                            </xsl:if>
                                          </xsl:for-each>
                                        </tr>
                                        <tbody class="scrollContent">
                                          <xsl:for-each select="activity/record">
                                            <tr >
                                              <td style="width:auto;">
                                                &#160;
                                              </td>
                                              <xsl:for-each select="attribute">
                                                <xsl:if test="@display!='false'">
                                                  <xsl:choose>
                                                    <xsl:when test="string-length(string(@value))>0">
                                                       <xsl:choose>                                                 		
                            		<xsl:when test="@type = 'number'">
                                          <td style="text-align:right;width:auto">
                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                          </td>
									</xsl:when>
                            		<xsl:otherwise>
                            			<td style="width:auto">
                                		<xsl:value-of select="@value"/>
                              			</td>
                            		</xsl:otherwise>
   								</xsl:choose>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                      <td>&#160;</td>
                                                    </xsl:otherwise>
                                                  </xsl:choose>
                                                </xsl:if>
                                              </xsl:for-each>
                                            </tr>
                                          </xsl:for-each>
                                        </tbody>
                                      </table>
                                    </td>
                                  </tr>
                                </xsl:if>
                              </xsl:for-each>
                            </tbody>
                          </table>
                        </td>
                      </tr>
                    </xsl:if>
                  </xsl:for-each>
                  </tbody>
                </table>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
      <!--End of table-->
      <Script language="javascript">
    setWindowTitle('Daily Wells Reporting');
      </Script>
    </div>
    </div>
  </xsl:template>
  <xsl:template name="ApplyToolTip">
    <xsl:param name="currentNode">
    </xsl:param>
    <xsl:attribute name="title">
    
   	<xsl:if test="string-length($currentNode/@tablename)&gt;0">
   	<xsl:text>Table Name: </xsl:text><xsl:value-of select="$currentNode/@tablename"></xsl:value-of><xsl:text>&#10;</xsl:text>  
	</xsl:if>

    <xsl:if test="string-length($currentNode/@dbcolumnname)&gt;0">
    <xsl:text>Column Name: </xsl:text><xsl:value-of select="$currentNode/@dbcolumnname"></xsl:value-of><xsl:text>&#10;</xsl:text>  
    </xsl:if>

    <xsl:if test="string-length($currentNode/@description)&gt;0">     
    <xsl:text>Formula: </xsl:text><xsl:value-of select="$currentNode/@description"></xsl:value-of><xsl:text>&#10;</xsl:text>  
    </xsl:if> 
    
    </xsl:attribute>
    </xsl:template>
</xsl:stylesheet>
