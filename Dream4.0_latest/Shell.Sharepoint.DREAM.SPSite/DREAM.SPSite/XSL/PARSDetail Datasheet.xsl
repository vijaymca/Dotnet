<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt"  xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <xsl:param name="LowRange"/>
  <xsl:param name="MediumRange"/>
  <xsl:param name="HighRange"/>
  <xsl:param name="LowRangeColor"/>
  <xsl:param name="MediumRangeColor"/>
  <xsl:param name="HighRangeColor"/>
  <!--Template to create the Response table.-->
  <xsl:template match="/">

    <!--Looping through each report-->
    <div id="tableContainer" style="none">
      <xsl:for-each select="response/report/record">
        <!--Creating Scrollbar.-->
        <!--table to display response data.-->
        <table class="RecordTable" cellpadding="4" cellspacing="0" id="Datasheet" >
          <!--Looping through all record having same report name.-->
          <xsl:for-each select="blocks/block">
            <!--<tr class="BlockRow">
              <td>
                <xsl:value-of select="@name"/>
              </td>
            </tr>-->
            <tr>
              <td>
                <table class="BlockTable" cellpadding="1" cellspacing="1" id="PARSData">
                  <tr class="GroupRow">
                    <xsl:for-each select="group">
                      <xsl:if test="@name='Identification'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='Source Application'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tr>
                    <xsl:for-each select="group">
                      <xsl:if test="@name='Identification'">
                        <td class="GroupColumn">
                          <table class="GroupTable" height="100%" cellpadding="4" cellspacing="0" id="Identification">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <td class="ValueColumn">
                                      <xsl:value-of select="@value"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='Source Application'">
                        <td class="GroupColumn">
                          <table class="GroupTable" height="100%" cellpadding="4" cellspacing="0" id="Source_Application">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <td class="ValueColumn">
                                      <xsl:value-of select="@value"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tr class="GroupRow">
                    <xsl:for-each select="group">
                      <xsl:if test="@name='Location'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='Outline of archive bounding box'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tr>
                    <xsl:for-each select="group">
                      <xsl:if test="@name='Location'">
                        <td class="GroupColumn">
                          <table class="GroupTable" cellpadding="4" cellspacing="0" id="Location">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <td class="ValueColumn">
                                      <xsl:value-of select="@value"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='Outline of archive bounding box'">
                        <td class="GroupColumn">
                          <table class="GroupTable" cellpadding="4" cellspacing="0" id="Outline_archive">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <td class="ValueColumn">
                                      <xsl:value-of select="@value"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tr class="GroupRow">
                    <xsl:for-each select="group">
                      <xsl:if test="@name='People'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='General'">
                        <td style="border:1px solid #BDBDBD;">
                          <xsl:call-template name="ApplyToolTip">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                          <xsl:value-of select="@name"/>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                  <tr>
                    <xsl:for-each select="group">
                      <xsl:if test="@name='People'">
                        <td class="GroupColumn">
                          <table class="GroupTable" cellpadding="4" cellspacing="0" id="People">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <td class="ValueColumn">
                                      <xsl:value-of select="@value"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                      <xsl:if test="@name='General'">
                        <td class="GroupColumn">
                          <table class="GroupTable" cellpadding="4" cellspacing="0" id="General">
                            <xsl:for-each select="attribute[@display != 'false']">
                              <tr>
                                <td class="NameColumn">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                                <xsl:choose>
                                  <xsl:when test="string(@value)">
                                    <xsl:choose>
                                      <xsl:when test="@type = 'date'">
                                        <td class="ValueColumn">
                                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                        </td>
                                      </xsl:when>
                                      <xsl:when test="@type = 'number'">
                                        <td style="text-align:right;" class="ValueColumn">
                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                        </td>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        <td class="ValueColumn">
                                          <xsl:value-of select="@value"/>
                                        </td>
                                      </xsl:otherwise>
                                    </xsl:choose>

                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="ValueColumn" style="background-color:#FFFFFF;color:white">
                                      -
                                    </td>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </tr>
                            </xsl:for-each>
                          </table>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </table>
              </td>
            </tr>
            <xsl:if test="@name='Quality'">
              <tr>
                <td>
                  <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                  </xsl:call-template>
                  <b>
                    <xsl:value-of select="@name"/>
                  </b>
                </td>
              </tr>
              <tr>
                <td>
                  <table class="BlockTable" cellpadding="1" cellspacing="1" id="Quality">
                    <tr class="GroupRow">
                      <xsl:for-each select="group">
                        <xsl:if test="@name='Record Quality'">
                          <td style="border:1px solid #BDBDBD;">
                            <xsl:call-template name="ApplyToolTip">
                              <xsl:with-param name="currentNode" select="."></xsl:with-param>
                            </xsl:call-template>
                            <xsl:value-of select="@name"/>
                          </td>
                        </xsl:if>
                      </xsl:for-each>
                    </tr>
                    <tr hieght="100%">
                      <xsl:for-each select="group">
                        <xsl:if test="@name='Record Quality'">
                          <td class="GroupColumn">
                            <table class="GroupTable" cellpadding="4" cellspacing="0">
                              <xsl:for-each select="attribute">
                                <tr>
                                  <td class="NameColumn">
                                    <xsl:call-template name="ApplyToolTip">
                                      <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                    </xsl:call-template>
                                    <xsl:value-of select="@name"/>
                                  </td>
                                  <xsl:choose>
                                    <xsl:when test="@name = 'Quality'">
                                      <xsl:call-template name="colorCode">
                                        <xsl:with-param name="value" select="@value"/>
                                        <xsl:with-param name="actualRange" select="@range"/>
                                      </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="string(@value)">
                                          <xsl:choose>
                                            <xsl:when test="@type = 'date'">
                                              <td >
                                                <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                              </td>
                                            </xsl:when>
                                            <xsl:when test="@type = 'number'">
                                              <td style="text-align:right;">
                                                <xsl:value-of select="format-number(@value, '#0.00')"/>
                                              </td>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <td >
                                                <xsl:value-of select="@value"/>
                                              </td>
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>
                                          <td>&#160;</td>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </tr>
                              </xsl:for-each>
                            </table>
                          </td>
                        </xsl:if>
                      </xsl:for-each>
                    </tr>
                  </table>
                </td>
              </tr>
            </xsl:if>
          </xsl:for-each>
        </table>
        <br></br>
        <!--End of table-->
      </xsl:for-each>
    </div>
    <script language="javascript" type="text/javascript">
      setWindowTitle('Project Archives Detail Report');
    </script>
  </xsl:template>
  <xsl:template name="colorCode">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="$value >= $HighRange">
        <xsl:choose>
          <xsl:when test="string(@value)">
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>
              </xsl:attribute>
              <xsl:value-of select="@value"/>&#160;
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>
              </xsl:attribute>
              &#160;
            </xsl:element>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$value >= $MediumRange">
            <xsl:choose>
              <xsl:when test="string(@value)">
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$MediumRangeColor'/>
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$MediumRangeColor'/>
                  </xsl:attribute>
                  &#160;
                </xsl:element>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="string(@value)">
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$LowRangeColor'/>
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$LowRangeColor'/>
                  </xsl:attribute>
                  &#160;
                </xsl:element>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="ApplyToolTip">
    <xsl:param name="currentNode">
    </xsl:param>
    <xsl:attribute name="title">

      <xsl:if test="string-length($currentNode/@tablename)&gt;0">
        <xsl:text>Table Name: </xsl:text>
        <xsl:value-of select="$currentNode/@tablename"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

      <xsl:if test="string-length($currentNode/@dbcolumnname)&gt;0">
        <xsl:text>Column Name: </xsl:text>
        <xsl:value-of select="$currentNode/@dbcolumnname"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

      <xsl:if test="string-length($currentNode/@description)&gt;0">
        <xsl:text>Formula: </xsl:text>
        <xsl:value-of select="$currentNode/@description"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

    </xsl:attribute>
  </xsl:template>
</xsl:stylesheet>