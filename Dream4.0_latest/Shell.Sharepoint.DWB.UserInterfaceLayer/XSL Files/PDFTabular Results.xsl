<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>
    <xsl:template match="/">
    <xsl:for-each select="response/report">
      <div>
        <table style="border-right:1px solid #336699;width:90%;"  cellpadding="0" cellspacing="0" 
          id="tblSearchResults">
          <xsl:for-each select="record[@recordno=1]">                   
              <tr id="headerSecondRow" style="height: 20px;background:  #e0ecf0;border:1px solid #336699;">
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title = 'true'">
                    <xsl:choose>
                      <xsl:when test="@name = 'Datum Pressure Unit'">
                        <th  width="50px"  style="border:1px solid #336699;">
                          <xsl:text>Datum Pressure Unit</xsl:text>
                        </th>
                      </xsl:when>
                      <xsl:when test="@name = 'Datum Depth AH'">
                        <th width="50px" style="border:1px solid #336699;">
                          <xsl:text>Datum Depth AH</xsl:text>
                        </th>
                      </xsl:when>
                      <xsl:otherwise>
                        <th style="border:1px solid #336699;">
                          <xsl:value-of select="@name"/>&#160;
                        </th>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                </xsl:for-each>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title != 'true'">
                    <!--<th>
                        <xsl:value-of select="@name"/>
                    </th>-->
                    <xsl:choose>
                      <xsl:when test="@name = 'Datum Pressure Unit'">
                        <th  width="50px" style="border:1px solid #336699;">
                          <xsl:text>Datum Pressure Unit</xsl:text>
                        </th>
                      </xsl:when>
                      <xsl:when test="@name = 'Datum Depth AH'">
                        <th width="50px" style="border:1px solid #336699;">
                          <xsl:text>Datum Depth AH</xsl:text>
                        </th>
                      </xsl:when>
                      <xsl:otherwise>
                        <th style="border:1px solid #336699;">
                          <xsl:value-of select="@name"/>&#160;
                        </th>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                </xsl:for-each>
              </tr>
                                       
            </xsl:for-each>          
          <!--Looping through all record having same report name.-->
          <tbody border="1" class="scrollContent">
            <xsl:for-each select="/response/report/record">
                <tr height="20px">                  
                  <xsl:for-each select="attribute[@display != 'false']">
                    <xsl:if test="@title = 'true'">
                      <xsl:choose>
                        <xsl:when test="string(@value)">
                          <xsl:choose>
                            <xsl:when test="@name = 'Datum Pressure Unit'">
                              <td width="50px">
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@name = 'Datum Depth AH'">
                              <td width="50px">
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:when>
                            <xsl:otherwise>
                              <td>
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
                          </xsl:choose>
                         
                        </xsl:when>
                        <xsl:otherwise>
                          <td width="1px">&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>                      
                    </xsl:if>
                  </xsl:for-each>
                  <xsl:for-each select="attribute[@display != 'false']">
                    <xsl:if test="@title != 'true'">
                      <xsl:choose>
                        <xsl:when test="string(@value)">
                          <xsl:choose>
                            <xsl:when test="@name = 'Datum Pressure Unit'">
                              <td width="50px">
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@name = 'Datum Depth AH'">
                              <td width="50px">
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:when>
                            <xsl:otherwise>
                              <td>
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <td width="1px">&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                  </xsl:for-each>
                </tr>                
              </xsl:for-each>
            </tbody>
          </table>
        </div>
      </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>
