<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt"  xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <xsl:param name="userPreference" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>
  <xsl:variable name="rowCounter" select="0"/>
  <xsl:variable name="rowId" select="''"/>
  <xsl:variable name="rowTotalDepths" select="'TotalDepths'"/>
  <xsl:param name="LowRange"/>
  <xsl:param name="MediumRange"/>
  <xsl:param name="HighRange"/>
  <xsl:param name="LowRangeColor"/>
  <xsl:param name="MediumRangeColor"/>
  <xsl:param name="HighRangeColor"/>
  
  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <div id="tableContainer" style="none">
      <table id="CompleteTable" width="100%" align="left">
        <tr>
          <td>
            <xsl:for-each select="response/report/record">
              <!--Creating Scrollbar.-->
              <xsl:variable name="rowCounter" select="$rowCounter + 1"/>
              <xsl:variable name="recordNumber" select="@recordno"/>
              <!--table to display response data.-->
              <table class="RecordTable" cellpadding="4" cellspacing="0" id="Datasheet">
                <!--Looping through all record having same report name.-->
                <xsl:for-each select="blocks/block">
                  <xsl:if test="@name='Well Information'">
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
                        <table class="BlockTable" cellpadding="1" cellspacing="1" id="WellData">
                          <tr class="GroupRow">
                            <xsl:for-each select="group">
                              <xsl:if test="@name='Identification'">
                                <td style="border:1px solid #9B9797;">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='General'">
                                <td style="border:1px solid #9B9797;">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                            </xsl:for-each>
                          </tr>
                          <tr height="100%">
                            <xsl:for-each select="group">
                              <xsl:if test="@name='Identification'">
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="Identification">
                                    <xsl:for-each select="attribute">
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
                                            <td class="ValueColumn">&#160;</td>
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
                                    <xsl:for-each select="attribute">
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
                                                <td class="ValueColumn">
                                                  <xsl:choose>
                                                    <xsl:when test="@value = 'No data'">
                                                      <xsl:value-of select="@value"/>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                      <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                    </xsl:otherwise>
                                                  </xsl:choose>
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
                                            <td class="ValueColumn">&#160;</td>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </tr>
                                    </xsl:for-each>
                                  </table>
                                </td>
                              </xsl:if>
                            </xsl:for-each>
                          </tr>
                          <xsl:for-each select="group">
                            <xsl:if test="@name='Location'">
                              <tr class="GroupRow">
                                <td style="border:1px solid #9B9797;" valign="top" colspan="2">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>

                                  <xsl:value-of select="@name"/>
                                </td>
                              </tr>
                              <tr>
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="LocationLeft">
                                    <xsl:for-each select="attribute">
                                      <xsl:choose>
                                        <xsl:when test="position()&lt;6">
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
                                                    <td class="ValueColumn">
                                                      <xsl:choose>
                                                        <xsl:when test="@value = 'No data'">
                                                          <xsl:value-of select="@value"/>
                                                        </xsl:when>
                                                        <xsl:otherwise>
                                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                        </xsl:otherwise>
                                                      </xsl:choose>
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
                                                <td class="ValueColumn">
                                                  &#160;
                                                </td>
                                              </xsl:otherwise>
                                            </xsl:choose>
                                          </tr>
                                        </xsl:when>
                                      </xsl:choose>
                                    </xsl:for-each>
                                  </table>
                                </td>
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="LocationRight">
                                    <xsl:for-each select="attribute">
                                      <xsl:choose>
                                        <xsl:when test="position()&gt;5">
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
                                                    <td class="ValueColumn">
                                                      <xsl:choose>
                                                        <xsl:when test="@value = 'No data'">
                                                          <xsl:value-of select="@value"/>
                                                        </xsl:when>
                                                        <xsl:otherwise>
                                                          <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                        </xsl:otherwise>
                                                      </xsl:choose>
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
                                                <td class="ValueColumn">
                                                  &#160;
                                                </td>
                                              </xsl:otherwise>
                                            </xsl:choose>
                                          </tr>
                                        </xsl:when>
                                      </xsl:choose>
                                    </xsl:for-each>
                                  </table>
                                </td>
                              </tr>
                            </xsl:if>
                          </xsl:for-each>
                        </table>
                      </td>
                    </tr>
                  </xsl:if>
                  <xsl:if test="@name='Wellbore Information'">
                    <tr class="BlockRow">
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
                        <table class="BlockTable" cellpadding="1" cellspacing="1" id="WellboreData">
                          <tr class="GroupRow">
                            <xsl:for-each select="group">
                              <xsl:if test="@name='Identification'">
                                <td style="border:1px solid #9B9797;">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>

                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='General'">
                                <td style="border:1px solid #9B9797;">
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
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="Identification">
                                    <xsl:for-each select="attribute">
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
                                            <td class="ValueColumn">
                                              &#160;
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
                                    <xsl:for-each select="attribute">
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
                                                <td class="ValueColumn">
                                                  <xsl:choose>
                                                    <xsl:when test="@value = 'No data'">
                                                      <xsl:value-of select="@value"/>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                      <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                    </xsl:otherwise>
                                                  </xsl:choose>
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
                                            <td class="ValueColumn">
                                              &#160;
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
                              <xsl:if test="@name='Depths'">
                                <td style="border:1px solid #9B9797;">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>

                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='Location'">
                                <td style="border:1px solid #9B9797;">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>

                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='Depth References'">
                                <td style="border:1px solid #9B9797;display:none">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>
                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                            </xsl:for-each>
                          </tr>
                          <tr id="trDepthRef">
                            <xsl:for-each select="group">
                              <xsl:if test="@name='Depths'">
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="Depths">
                                    <xsl:for-each select="attribute[@display != 'false']">
                                      <tr>
                                        <td class="NameColumn">
                                          <xsl:call-template name="ApplyToolTip">
                                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                          </xsl:call-template>
                                          <xsl:value-of select="@name"/>
                                        </td>
                                        <xsl:choose>
                                          <xsl:when test="string(@referencecolumn)">
                                            <xsl:variable name="ActualColumnName" select="@name"/>
                                            <xsl:variable name="referencecolumnName" select="@referencecolumn"/>
                                            <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/blocks/block/group[@name='Depths']/attribute[@name = $referencecolumnName]">
                                              <xsl:variable name="unitColumnName" select="@name"/>
                                              <xsl:variable name="unitValue" select="@value"/>
                                              <xsl:choose>
                                                <xsl:when test="$unitValue = $userPreference">
                                                  <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/blocks/block/group[@name='Depths']/attribute[@name = $ActualColumnName]">
                                                    <xsl:choose>
                                                      <xsl:when test="string(@value)">
                                                        <xsl:variable name="rowTotalDepths" select="@name" />
                                                        <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                        <xsl:element name="td">
                                                          <xsl:attribute name="id">
                                                            <xsl:value-of select="$rowId"/>
                                                          </xsl:attribute>
                                                          <xsl:attribute name="class">ValueColumn</xsl:attribute>
                                                          <xsl:if test="$userPreference = 'metres'">
                                                            <xsl:choose>
                                                              <xsl:when test="@value = 'No data'">
                                                                <xsl:value-of select="@value"/>
                                                              </xsl:when>
                                                              <xsl:otherwise>
                                                                <xsl:value-of select="format-number(@value,'#0.00')"/>&#160;(m)
                                                              </xsl:otherwise>
                                                            </xsl:choose>
                                                          </xsl:if>
                                                          <xsl:if test="$userPreference = 'feet'">
                                                            <xsl:choose>
                                                              <xsl:when test="@value = 'No data'">
                                                                <xsl:value-of select="@value"/>
                                                              </xsl:when>
                                                              <xsl:otherwise>
                                                                <xsl:value-of select="format-number(@value,'#0.00')"/>&#160;(ft)
                                                              </xsl:otherwise>
                                                            </xsl:choose>
                                                          </xsl:if>
                                                        </xsl:element>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:variable name="rowTotalDepths" select="@name" />
                                                        <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                        <xsl:element name="td">
                                                          <xsl:attribute name="id">
                                                            <xsl:value-of select="$rowId"/>
                                                          </xsl:attribute>
                                                          <xsl:attribute name="class">ValueColumn</xsl:attribute>&#160;
                                                        </xsl:element>
                                                      </xsl:otherwise>
                                                    </xsl:choose>
                                                  </xsl:for-each>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/blocks/block/group[@name='Depths']/attribute[@name = $ActualColumnName]">
                                                    <xsl:choose>
                                                      <xsl:when test="string(@value)">
                                                        <xsl:variable name="rowTotalDepths" select="@name" />
                                                        <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                        <xsl:element name="td">
                                                          <xsl:attribute name="id">
                                                            <xsl:value-of select="$rowId"/>
                                                          </xsl:attribute>
                                                          <xsl:attribute name="class">ValueColumn</xsl:attribute>
                                                          <xsl:if test="$userPreference = 'metres'">
                                                            <xsl:choose>
                                                              <xsl:when test="@value = 'No data'">
                                                                <xsl:value-of select="@value"/>
                                                              </xsl:when>
                                                              <xsl:otherwise>
                                                                <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>&#160;(m)
                                                              </xsl:otherwise>
                                                            </xsl:choose>
                                                          </xsl:if>
                                                          <xsl:if test="$userPreference = 'feet'">
                                                            <xsl:choose>
                                                              <xsl:when test="@value = 'No data'">
                                                                <xsl:value-of select="@value"/>
                                                              </xsl:when>
                                                              <xsl:otherwise>
                                                                <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>&#160;(ft)
                                                              </xsl:otherwise>
                                                            </xsl:choose>
                                                          </xsl:if>
                                                        </xsl:element>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:variable name="rowTotalDepths" select="@name" />
                                                        <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                        <xsl:element name="td">
                                                          <xsl:attribute name="id">
                                                            <xsl:value-of select="$rowId"/>
                                                          </xsl:attribute>
                                                          <xsl:attribute name="class">ValueColumn</xsl:attribute>&#160;
                                                        </xsl:element>
                                                      </xsl:otherwise>
                                                    </xsl:choose>
                                                  </xsl:for-each>
                                                </xsl:otherwise>
                                              </xsl:choose>
                                            </xsl:for-each>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:choose>
                                              <xsl:when test="string(@value)">
                                                <xsl:variable name="rowTotalDepths" select="@name" />
                                                <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                <xsl:element name="td">
                                                  <xsl:attribute name="id">
                                                    <xsl:value-of select="$rowId"/>
                                                  </xsl:attribute>
                                                  <xsl:attribute name="class">ValueColumn</xsl:attribute>
                                                  <xsl:if test="$userPreference = 'metres'">
                                                    <xsl:value-of select="@value"/>&#160;
                                                  </xsl:if>
                                                  <xsl:if test="$userPreference = 'feet'">
                                                    <xsl:value-of select="@value"/>&#160;
                                                  </xsl:if>
                                                </xsl:element>
                                              </xsl:when>
                                              <xsl:otherwise>
                                                <xsl:variable name="rowTotalDepths" select="@name" />
                                                <xsl:variable name="rowId" select="concat($rowTotalDepths, $rowCounter)" />
                                                <xsl:element name="td">
                                                  <xsl:attribute name="id">
                                                    <xsl:value-of select="$rowId"/>
                                                  </xsl:attribute>
                                                  <xsl:attribute name="class">ValueColumn</xsl:attribute>&#160;
                                                </xsl:element>
                                              </xsl:otherwise>
                                            </xsl:choose>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </tr>
                                    </xsl:for-each>
                                  </table>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='Location'">
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="Location">
                                    <xsl:for-each select="attribute">
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
                                                <td class="ValueColumn">
                                                  <xsl:choose>
                                                    <xsl:when test="@value = 'No data'">
                                                      <xsl:value-of select="@value"/>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                      <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                    </xsl:otherwise>
                                                  </xsl:choose>
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
                                            <td class="ValueColumn" >
                                              &#160;
                                            </td>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </tr>
                                    </xsl:for-each>
                                  </table>
                                </td>
                              </xsl:if>
                              <xsl:if test="@name='Depth References'">
                                <td class="GroupColumn">
                                  <xsl:attribute name="style">
                                    display:none
                                  </xsl:attribute>
                                  <table class="GroupTable" cellpadding="4" cellspacing="0" id="Depth References">
                                    <xsl:for-each select="attribute">
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
                                              <xsl:call-template name="AddTVDSSCell">
                                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                              </xsl:call-template>
                                            </td>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <td class="ValueColumn" >
                                              &#160;
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
                  </xsl:if>
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
                          <tr class="GroupRow1">
                            <xsl:for-each select="group">
                              <xsl:if test="@name='Record Quality'">
                                <td style="border:1px solid #9B9797;" valign="top" colspan="2">
                                  <xsl:call-template name="ApplyToolTip">
                                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                  </xsl:call-template>

                                  <xsl:value-of select="@name"/>
                                </td>
                              </xsl:if>
                            </xsl:for-each>
                          </tr>
                          <tr height="100%">
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
                                                <td class="ValueColumn">
                                                  <xsl:value-of select="@value"/>
                                                </td>
                                              </xsl:when>
                                              <xsl:otherwise>
                                                <td class="ValueColumn">&#160;</td>
                                              </xsl:otherwise>
                                            </xsl:choose>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </tr>
                                    </xsl:for-each>
                                  </table>
                                </td>
                                <td class="GroupColumn">
                                  <table class="GroupTable" cellpadding="4" cellspacing="0">
                                    <xsl:for-each select="attribute">
                                      <tr>
                                        <td class="NameColumn1">
                                        </td>
                                        <td>&#160;</td>
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
                  <xsl:if test="@name='Field Information' or @name='Reservoir Information'">
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
                        <table class="BlockTable" cellpadding="1" cellspacing="1" id="FieldData">

                          <xsl:for-each select="group">

                            <tr class="GroupRow">
                              <td style="border:1px solid #9B9797;" colspan="2">
                                <xsl:call-template name="ApplyToolTip">
                                  <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                </xsl:call-template>
                                <xsl:value-of select="@name"/>
                              </td>
                            </tr>                            
                            <tr>                              
                              <xsl:variable name="noOfAttributes" select="count(attribute)"></xsl:variable>
                              <xsl:variable name="noOfRows" select="ceiling(count(attribute) div 2)"/>                             
                              <td class="GroupColumn">
                                <table class="GroupTable" cellpadding="4" cellspacing="0" id="LocationLeft">
                                  <xsl:for-each select="attribute">                                    
                                    <xsl:choose>
                                      <xsl:when test = "$noOfAttributes = 1">
                                        <tr>
                                          <td class="NameColumn" nowrap="nowrap">
                                            <xsl:call-template name="ApplyToolTip">
                                              <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                            </xsl:call-template>

                                            <xsl:value-of select="@name"/>
                                          </td>
                                          <xsl:choose>
                                            <xsl:when test="string(@value)">
                                              <xsl:choose>
                                                <xsl:when test="@type = 'date'">
                                                  <td class="ValueColumn" colspan="3" nowrap="nowrap">
                                                    <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                                  </td>
                                                </xsl:when>
                                                <xsl:when test="@type = 'number'">
                                                  <td class="ValueColumn" colspan="3" nowrap="nowrap">
                                                    <xsl:choose>
                                                      <xsl:when test="@value = 'No data'">
                                                        <xsl:value-of select="@value"/>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                      </xsl:otherwise>
                                                    </xsl:choose>
                                                  </td>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <td class="ValueColumn" colspan="3" nowrap="nowrap">
                                                    <xsl:value-of select="@value"/>
                                                  </td>
                                                </xsl:otherwise>
                                              </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <td class="ValueColumn" colspan="3" nowrap="nowrap">
                                                &#160;
                                              </td>
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </tr>
                                      </xsl:when>
                                      <xsl:when test="position() &lt;= $noOfRows">
                                        <tr>
                                          <td class="NameColumn" nowrap="nowrap">
                                            <xsl:call-template name="ApplyToolTip">
                                              <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                            </xsl:call-template>

                                            <xsl:value-of select="@name"/>
                                          </td>
                                          <xsl:choose>
                                            <xsl:when test="string(@value)">
                                              <xsl:choose>
                                                <xsl:when test="@type = 'date'">
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                                  </td>
                                                </xsl:when>
                                                <xsl:when test="@type = 'number'">
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:choose>
                                                      <xsl:when test="@value = 'No data'">
                                                        <xsl:value-of select="@value"/>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                      </xsl:otherwise>
                                                    </xsl:choose>
                                                  </td>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:value-of select="@value"/>
                                                  </td>
                                                </xsl:otherwise>
                                              </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <td class="ValueColumn" nowrap="nowrap">
                                                &#160;
                                              </td>
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </tr>

                                      </xsl:when>

                                    </xsl:choose>
                                  </xsl:for-each>
                                </table>
                              </td>
                              <td class="GroupColumn">
                                <table class="GroupTable" cellpadding="4" cellspacing="0" id="LocationRight">
                                  <xsl:for-each select="attribute">                                 
                                    <xsl:choose>
                                      <xsl:when test="position()&gt; $noOfRows">
                                        <tr>
                                          <td class="NameColumn" nowrap="nowrap">
                                            <xsl:call-template name="ApplyToolTip">
                                              <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                            </xsl:call-template>

                                            <xsl:value-of select="@name"/>
                                          </td>
                                          <xsl:choose>
                                            <xsl:when test="string(@value)">
                                              <xsl:choose>
                                                <xsl:when test="@type = 'date'">
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                                  </td>
                                                </xsl:when>
                                                <xsl:when test="@type = 'number'">
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:choose>
                                                      <xsl:when test="@value = 'No data'">
                                                        <xsl:value-of select="@value"/>
                                                      </xsl:when>
                                                      <xsl:otherwise>
                                                        <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                      </xsl:otherwise>
                                                    </xsl:choose>
                                                  </td>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                  <td class="ValueColumn" nowrap="nowrap">
                                                    <xsl:value-of select="@value"/>
                                                  </td>
                                                </xsl:otherwise>
                                              </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <td class="ValueColumn" nowrap="nowrap">
                                                &#160;
                                              </td>
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </tr>                                                                               
                                      </xsl:when>
                                    </xsl:choose>
                                  </xsl:for-each>
                                </table>
                              </td>
                            </tr>                            
                          </xsl:for-each>

                        </table>
                      </td>
                    </tr>
                  </xsl:if>
                </xsl:for-each>
              </table>
              <br></br>
              <!--End of table-->
            </xsl:for-each>
          </td>
        </tr>
      </table>
    </div>
  </xsl:template>
  <xsl:template name="colorCode">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="$value >= $HighRange">
        <xsl:choose>
          <xsl:when test="string(@value)">
            <td class="ValueColumn1">
              <table>
                <tr>
                  <xsl:element name="td">
                    <xsl:attribute name="style">
                      background-color:<xsl:value-of select='$HighRangeColor'/>;
                      font-weight:bold;
                      text-align:center;
                    </xsl:attribute>
                    &#160;&#160;<xsl:value-of select="@value"/>&#160;&#160;
                  </xsl:element>
                </tr>
              </table>
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="ValueColumn1">&#160;</td>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$value >= $MediumRange">
            <xsl:choose>
              <xsl:when test="string(@value)">
                <td class="ValueColumn1">
                  <table>
                    <tr>
                      <xsl:element name="td">
                        <xsl:attribute name="style">
                          background-color:<xsl:value-of select='$MediumRangeColor'/>;
                          font-weight:bold;
                          text-align:center;
                        </xsl:attribute>
                        &#160;&#160;<xsl:value-of select="@value"/>&#160;&#160;
                      </xsl:element>
                    </tr>
                  </table>
                </td>
              </xsl:when>
              <xsl:otherwise>
                <td class="ValueColumn1">&#160;</td>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="string(@value)">
                <td class="ValueColumn1">
                  <table border="1px">
                    <tr>
                      <xsl:element name="td">
                        <xsl:attribute name="style">
                          background-color:<xsl:value-of select='$LowRangeColor'/>;
                          font-weight:bold;
                          text-align:center;
                        </xsl:attribute>
                        &#160;&#160;<xsl:value-of select="@value"/>&#160;&#160;
                      </xsl:element>
                    </tr>
                  </table>
                </td>
              </xsl:when>
              <xsl:otherwise>
                <td class="ValueColumn1">&#160;</td>
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
  <xsl:template name="AddTVDSSCell">
    <xsl:param name="currentNode"/>
    <xsl:choose>
      <xsl:when test="$currentNode/@name ='Depth References'">
        <xsl:attribute name="style">
          display:none
        </xsl:attribute>
        <xsl:value-of select="$currentNode/@value"/><xsl:value-of select="($currentNode/@default)"></xsl:value-of>|<xsl:value-of select="($currentNode/@unit)"></xsl:value-of>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$currentNode/@value"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="HideColumn">
    <xsl:param name="currentNode"/>
    <xsl:choose>
      <xsl:when test="$currentNode/@name ='Depth References'">
        <xsl:attribute name="style">
          display:none
        </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>