<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:objDate="urn:DATE">
  <xsl:param name="date" select="''"/>
  <xsl:variable name="title">
    Well Summary of <xsl:value-of select="/response/report/record[@datasource='RSM']/blocks/block[@name='Well Information']/group[@name='Header']/attribute/@value"/>
  </xsl:variable>
  <xsl:variable name="status">
    <xsl:value-of select="/response/report/record[@datasource='RSM']/blocks/block/group[@name='Key Well Information']/attribute[@name = 'pro_inj']/@value"/>
  </xsl:variable>
  <xsl:template match="/">
    <link rel="stylesheet" type="text/css" href="_layouts/DREAM/styles/WellHistoryStyle.css" />
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="response">
    <div id="tableContainer">
      <span style="width: 100%; font-weight: bold; font-size: 1.4em; color: #33339F;">
        <xsl:value-of select="$title"/>
      </span>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Key Well Information [RSM]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Key Well Information']">
          <table width="100%">
            <xsl:for-each select="/response/report/record/blocks/block/group[@name='Key Well Information']/attribute[@display != 'false']">
              <xsl:if test="@name != 'Last Date'">
              <tr>
                <xsl:call-template name="OddEvenColors"/>
                <td width="20%" valign="top">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="80%" valign="top">
                  <xsl:choose>
                    <xsl:when test="string(@value)">
                      <xsl:choose>
                        <xsl:when test="@type = 'date'">
                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@value"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <td>&#160;</td>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
              </tr>
              </xsl:if>
              <xsl:if test="@name = 'Last Date'">
                <xsl:choose>
                  <xsl:when test="$status = 'P'">
                    <tr>
                      <xsl:call-template name="OddEvenColors"/>
                      <td width="20%" valign="top">
                        Last Prod Date
                      </td>
                      <td width="80%" valign="top">
                        <xsl:choose>
                          <xsl:when test="string(@value)">
                            <xsl:choose>
                              <xsl:when test="@type = 'date'">
                                <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="@value"/>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>&#160;</td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </xsl:when>
                  <xsl:otherwise>
                    <tr>
                      <xsl:call-template name="OddEvenColors"/>
                      <td width="20%" valign="top">
                        Last Inj Date
                      </td>
                      <td width="80%" valign="top">
                        <xsl:choose>
                          <xsl:when test="string(@value)">
                            <xsl:choose>
                              <xsl:when test="@type = 'date'">
                                <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="@value"/>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>&#160;</td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </xsl:otherwise>
                </xsl:choose>                
              </xsl:if>             
            </xsl:for-each>
          </table>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <xsl:choose>
              <xsl:when test="$status = 'P'">
                <b>Production Data [RSM]</b>
              </xsl:when>
              <xsl:otherwise>
                <b>Injection Data [RSM]</b>
              </xsl:otherwise>
            </xsl:choose>
            
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Production Or Injection Data']">
          <table width="100%">
            <xsl:for-each select="/response/report/record/blocks/block/group[@name='Production Or Injection Data']/attribute[@display != 'false']">
              <tr>
                <xsl:call-template name="OddEvenColors"/>
                <td width="20%" valign="top">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="80%" valign="top">
                  <xsl:choose>
                    <xsl:when test="string(@value)">
                      <xsl:choose>
                        <xsl:when test="@type = 'date'">
                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@value"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <td>&#160;</td>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Last Valid Well Test [RSM]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Last Valid Well Test']">          
          <table width="100%">
            <tr>
              <td>
                <table width="100%">
                  <tr>
                    <td class="unit" width="15%"></td>
                    <td class="unit" width="5%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Choke Unit']/@value"/>
                    </td>
                    <td class="unit" width="10%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FTHP Unit']/@value"/>
                    </td>
                    <td class="unit" width="10%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FTHT Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Oil Rate Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Gas Rate Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Water Rate Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Gas Lift Rate Unit']/@value"/>
                    </td>
                  </tr>
                  <tr class="heading">
                    <td class="owner" width="15%">
                      Date
                    </td>
                    <td class="owner" width="5%">
                      Choke
                    </td>
                    <td class="owner" width="10%">
                      FTHP
                    </td>
                    <td class="owner" width="10%">
                      FTHT
                    </td>
                    <td class="owner" width="15%">
                      Net Oil Rate
                    </td>
                    <td class="owner" width="15%">
                      Net Gas Rate
                    </td>
                    <td class="owner" width="15%">
                      Net Water Rate
                    </td>
                    <td class="owner" width="15%">
                      Gas Lift Rate
                    </td>
                  </tr>
                  <tr>
                    <td class="owner" width="15%">
                      <xsl:choose>
                        <xsl:when test="string(/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Date']/@value)">
                          <xsl:value-of select="objDate:GetDateTime(/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Date']/@value)"/>

                        </xsl:when>
                        <xsl:otherwise>
                          &#160;
                        </xsl:otherwise>
                      </xsl:choose>                      
                    </td>
                    <td class="owner" width="5%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Choke']/@value"/>
                    </td>
                    <td class="owner" width="10%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FTHP']/@value"/>
                    </td>
                    <td class="owner" width="10%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FTHT']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Oil Rate']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Gas Rate']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Net Water Rate']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Gas Lift Rate']/@value"/>
                    </td>
                  </tr>
                </table>                      
              </td>
            </tr>

            <tr>
              <td>
                <table width="100%">
                  <tr>
                    <td class="unit" width="10%"></td>
                    <td class="unit" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FGOR Unit']/@value"/>
                    </td>
                    <td class="unit" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='TGLR Unit']/@value"/>
                    </td>
                    <td class="unit" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Water-Cut Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='BHP Unit']/@value"/>
                    </td>
                    <td class="unit" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='BHT Unit']/@value"/>
                    </td>
                  </tr>
                  <tr class="heading">
                    <td class="owner" width="10%"></td>
                    <td class="owner" width="20%">FGOR</td>
                    <td class="owner" width="20%">
                      TGLR
                    </td>
                    <td class="owner" width="20%">
                      Water-Cut
                    </td>
                    <td class="owner" width="15%">
                      BHP
                    </td>
                    <td class="owner" width="15%">
                      BHT
                    </td>
                  </tr>
                  <tr>
                    <td class="owner" width="10%"></td>
                    <td class="owner" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='FGOR']/@value"/>
                    </td>
                    <td class="owner" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='TGLR']/@value"/>
                    </td>
                    <td class="owner" width="20%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='Water-Cut']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='BHP']/@value"/>
                    </td>
                    <td class="owner" width="15%">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Last Valid Well Test']/attribute[@name='BHT']/@value"/>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>                          
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Well Integrity [RSM]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Well Integrity']">
          <table width="100%">
            <xsl:for-each select="/response/report/record/blocks/block/group[@name='Well Integrity']/attribute[@display != 'false']">
              <tr>
                <xsl:call-template name="OddEvenColors"/>
                <td width="20%" valign="top">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="80%" valign="top">
                  <xsl:choose>
                    <xsl:when test="string(@value)">
                      <xsl:choose>
                        <xsl:when test="@type = 'date'">
                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@value"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <td>&#160;</td>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Last HUD [RSM]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Last HUD']">
          <table width="100%">
            <xsl:for-each select="/response/report/record/blocks/block/group[@name='Last HUD']/attribute[@display != 'false']">
              <tr>
                <xsl:call-template name="OddEvenColors"/>
                <td width="20%" valign="top">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="80%" valign="top">
                  <xsl:choose>
                    <xsl:when test="string(@value)">
                      <xsl:choose>
                        <xsl:when test="@type = 'date'">
                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@value"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <td>&#160;</td>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Last PTA result [RSM]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record/blocks/block/group[@name='Last PTA result']">
          <table width="100%">
            <xsl:for-each select="/response/report/record/blocks/block/group[@name='Last PTA result']/attribute[@display != 'false']">
              <tr>
                <xsl:call-template name="OddEvenColors"/>
                <td width="20%" valign="top">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="80%" valign="top">
                  <xsl:choose>
                    <xsl:when test="string(@value)">
                      <xsl:choose>
                        <xsl:when test="@type = 'date'">
                          <xsl:value-of select="objDate:GetDateTime(@value)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@value"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <td>&#160;</td>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
              </tr>
            </xsl:for-each>
          </table>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>
      <BR></BR>
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.2em;">
            <b>Latest Well Review [SWED]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
      <xsl:choose>
        <xsl:when test="/response/report/record[@datasource='SWED']">
          <xsl:choose>
            <xsl:when test="/response/report/record/blocks/block/group[@name='Review']">
              <table width="100%">
                <xsl:for-each select="/response/report/record/blocks/block/group[@name='Review']/attribute[@display != 'false']">
                  <tr>
                    <xsl:call-template name="OddEvenColors"/>
                    <td width="20%" valign="top">
                      <xsl:value-of select="@name"/>
                    </td>
                    <td width="80%" valign="top">
                      <xsl:choose>
                        <xsl:when test="string(@value)">
                          <xsl:choose>
                            <xsl:when test="@type = 'date'">
                              <xsl:value-of select="objDate:GetDateTime(@value)"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="@value"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <td>&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:when>
          </xsl:choose>
          <xsl:choose>
            <xsl:when test="/response/report/record/blocks/block/group[@name='Action']">
              <table width="100%">
                <tr>
                  <td class="Unit" width="5%" valign="top"></td>
                  <td class="Unit" width="10%" valign="top"></td>
                  <td class="Unit" width="10%" valign="top"></td>
                  <td class="Unit" width="10%" valign="top"></td>
                  <td class="Unit" width="35%" valign="top"></td>
                  <td class="Unit" width="10%" valign="top">
                    <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Gain Unit']/@value"/>
                  </td>
                  <td class="Unit" width="10%" valign="top">
                    <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='POS Unit']/@value"/>
                  </td>
                  <td class="Unit" width="10%" valign="top">
                    <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Cost Unit']/@value"/>
                  </td>
                </tr>
                <tr class="heading">
                  <td width="5%" valign="top">#</td>
                  <td width="10%" valign="top">Action</td>
                  <td width="10%" valign="top">Timing</td>
                  <td width="10%" valign="top">Resource</td>
                  <td width="35%" valign="top">Comments</td>
                  <td width="10%" valign="top">Gain</td>
                  <td width="10%" valign="top">POS</td>
                  <td width="10%" valign="top">Cost</td>
                </tr>
                <xsl:for-each select="/response/report/record/blocks/block/group[@name='Action']/record">
                  <tr>
                    <xsl:call-template name="OddEvenColors"/>
                    <td class="number" width="5%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Action #']/@value"/>
                    </td>
                    <td width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Action']/@value"/>
                    </td>
                    <td class="timing" width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Timing']/@value"/>
                    </td>
                    <td class="resource" width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Resource']/@value"/>
                    </td>
                    <td width="35%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Comments']/@value"/>
                    </td>
                    <td class="gain" width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Gain']/@value"/>
                    </td>
                    <td class="pos" width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='POS']/@value"/>
                    </td>
                    <td class="cost" width="10%" valign="top">
                      <xsl:value-of select="/response/report/record/blocks/block/group[@name='Action']/record/attribute[@name='Cost']/@value"/>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:when>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="nodata"/>
        </xsl:otherwise>
      </xsl:choose>      
    </div>
  </xsl:template>
  
  <xsl:template name="OddEvenColors">
    <xsl:param name="odd">1</xsl:param>
    <xsl:attribute name="class">
      <xsl:choose>
        <xsl:when test="position() mod 2 = 1">odd</xsl:when>
        <xsl:otherwise>even</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <xsl:template name="nodata">
    <table>
      <tbody>
        <tr>
          <td style="color:red">No information available</td>
        </tr>
      </tbody>
    </table>
  </xsl:template>
</xsl:stylesheet>