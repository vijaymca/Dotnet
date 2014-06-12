<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:objDate="urn:DATE">
  <xsl:param name="date" select="''"/>
	<xsl:variable name="EDMdiv2">
		<xsl:value-of select="(1 + count(/OverViewWell/Q2/row)) div 2"/>
	</xsl:variable>
	<xsl:variable name="title">Well history of <xsl:value-of select="/OverViewWell/@HOLE"/>
	</xsl:variable>
	<xsl:template match="/">
    <link rel="stylesheet" type="text/css" href="/_layouts/DREAM/styles/WellHistoryStyleRel2_1.css" />			
			<xsl:apply-templates/>
	</xsl:template>
	<xsl:template match="OverViewWell">
    <div id="tableContainer">
    <span style="width: 100%; font-weight: bold; font-size: 1.6em; color: #33339F;">
			<xsl:value-of select="$title"/>
    </span>
      <BR></BR>
    <!--<span style="width: 100%;font-size: 0.9em;">
      <xsl:variable name="currentDate" select="objDate:GetDateTimeZone('12-12-12')"/>
      Created from source databases on : <xsl:value-of select="$currentDate"/>&#160;
    </span>-->
    <br></br>    
      <table style="border-top: thin black solid;"  width="100%">
        <tr>
          <td style="font-size: 1.3em;">
            <b>Well Entries [EDM Events]</b>
          </td>
        </tr>
      </table>
      <BR></BR>      
        <xsl:choose>
          <xsl:when test="Q2">
            <table width="100%">
              <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <xsl:attribute name="class">heading</xsl:attribute>
                      <th>Date</th>
                      <th>Objective</th>
                    </tr>
                    <xsl:for-each select="Q2/row">
                      <xsl:if test="not(position()>$EDMdiv2)">
                        <tr>
                          <xsl:call-template name="OddEvenColors"/>
                          <td class="date">
                            <xsl:value-of select="@DATE"/>
                          </td>
                          <td>
                            <xsl:value-of select="@CODE"/>
                          </td>
                        </tr>
                      </xsl:if>
                    </xsl:for-each>
                  </table>
                </td>
                <td>
                  <table width="100%">
                    <tr>
                      <xsl:attribute name="class">heading</xsl:attribute>
                      <th>Date</th>
                      <th>Objective</th>
                    </tr>
                    <xsl:for-each select="Q2/row">
                      <xsl:if test="position()>$EDMdiv2">
                        <tr>
                          <xsl:call-template name="OddEvenColors"/>
                          <td class="date">
                            <xsl:value-of select="@DATE"/>
                          </td>
                          <td>
                            <xsl:value-of select="@CODE"/>
                          </td>
                        </tr>
                      </xsl:if>
                    </xsl:for-each>
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
          <td style="font-size: 1.3em;">
            <b>Well History [SWED Events with priority 'H']</b>
          </td>
        </tr>
      </table>      
      <BR></BR>
        <xsl:choose>
          <xsl:when test="Q1">
            <table width="100%">
              <tr>
                <xsl:attribute name="class">heading</xsl:attribute>
                <th>Date</th>
                <th>Type</th>
                <th>Comment</th>
                <th>Owner</th>
              </tr>
              <xsl:for-each select="Q1/row">
                <tr>
                  <xsl:call-template name="OddEvenColors"/>
                  <td class="date">
                    <xsl:value-of select="@DATE"/>
                  </td>
                  <td class="type">
                    <xsl:value-of select="@TYPE"/>
                  </td>
                  <td>
                    <xsl:value-of select="@COMMENTS"/>
                  </td>
                  <td class="owner">
                    <xsl:value-of select="@OWNER"/>
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
          <td style="font-size: 1.3em;">
            <b>Latest Well Review [SWED]</b>
          </td>
        </tr>
      </table>
      <BR></BR>
        <xsl:choose>
          <xsl:when test="Q3">
            <table width="100%">
              <tr>
                <td colspan="2" class="head">Date</td>
                <td colspan="6" class="data">
                  <xsl:value-of select="Q3/row/@DATE"/>
                </td>
              </tr>
              <tr>
                <td colspan="2" class="head">Observation</td>
                <td colspan="6" class="data">
                  <xsl:value-of select="Q3/row/@OBSERVATION"/>
                </td>
              </tr>
              <tr>
                <td colspan="2" class="head">Objective</td>
                <td colspan="6" class="data">
                  <xsl:value-of select="Q3/row/@OBJECTIVE"/>
                </td>
              </tr>
              <tr>
                <th/>
                <th/>
                <th/>
                <th/>
                <th/>
                <td class="unit">
                  <xsl:value-of select="Q3/row/@GAINUNIT"/>
                </td>
                <td class="unit">
                  <xsl:value-of select="Q3/row/@POSUNIT"/>
                </td>
                <td class="unit">
                  <xsl:value-of select="Q3/row/@CURRENCY"/>
                </td>
              </tr>
              <tr>
                <xsl:attribute name="class">heading</xsl:attribute>
                <th>#</th>
                <th>Action</th>
                <th>Timing</th>
                <th>Resource</th>
                <th>Comments</th>
                <th>Gain</th>
                <th>Pos</th>
                <th>Cost</th>
              </tr>
              <xsl:for-each select="Q3/row">
                <tr>
                  <xsl:call-template name="OddEvenColors"/>
                  <td class="number">
                    <xsl:value-of select="@NUMBER"/>
                  </td>
                  <td>
                    <xsl:value-of select="@ACTION"/>
                  </td>
                  <td class="timing">
                    <xsl:value-of select="@TIMING"/>
                  </td>
                  <td class="resource">
                    <xsl:value-of select="@WHO"/>
                  </td>
                  <td>
                    <xsl:value-of select="@WHAT"/>
                  </td>
                  <td class="gain">
                    <xsl:value-of select="@GAIN"/>
                  </td>
                  <td class="pos">
                    <xsl:value-of select="@POS"/>
                  </td>
                  <td class="cost">
                    <xsl:value-of select="@COST"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="nodata"/>
          </xsl:otherwise>
        </xsl:choose>
    </div>
	</xsl:template>
	<xsl:template name="OddEvenColors">
		<xsl:param name="odd">1</xsl:param>
		<xsl:attribute name="class"><xsl:choose><xsl:when test="position() mod 2 = 1">odd</xsl:when><xsl:otherwise>even</xsl:otherwise></xsl:choose></xsl:attribute>
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