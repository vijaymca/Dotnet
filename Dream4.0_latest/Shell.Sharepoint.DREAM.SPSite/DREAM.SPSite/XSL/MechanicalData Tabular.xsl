<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt2="urn:frontpage:internal" xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <!--Parameter declaration start -->
  <!--Display Unit Value-->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="activeDiv"/>
  <!--Parameter declaration end -->

  <!--Template to create the Response table.-->
  <xsl:template match="/" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">
    <!--table to display response data.-->
    <!--table to display Well header data.-->
    <div>
      <table id="tblHeader" style="border-right:1px solid #bdbdbd" cellpadding="0" cellspacing="0" >
        <tr>
          <xsl:call-template name="CreateHeaderRow">
            <xsl:with-param name="isData" select="'true'"/>
            <xsl:with-param name="childColl" select="response/report/record/hierarchy"/>
          </xsl:call-template>
        </tr>
        <tbody class="scrollContent" id="scrollContent">
          <xsl:call-template name="CreateDataRows">
            <xsl:with-param name="childColl" select="response/report/record/hierarchy"/>
          </xsl:call-template>
        </tbody>
      </table>
    </div>
    <div class="gray_embossed_tabs_r" >
      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
          <td>
            <ul class="tabNavigation">
              <li>
                <a>
                  <span id="lblholesection" class="holesection" style="cursor:hand;" onclick="javascript:MDRTabClick('holesection');">Hole Sections</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblcasings" class="casings" style="cursor:hand" onclick="javascript:MDRTabClick('casings');">Casings</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblliners" class="liners" style="cursor:hand" onclick="javascript:MDRTabClick('liners');">Liners</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblmechanicalcontent" class="mechanicalcontent" style="cursor:hand" onclick="javascript:MDRTabClick('mechanicalcontent');">Mechanical Content</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblfluidscements" class="fluidscements" style="cursor:hand" onclick="javascript:MDRTabClick('fluidscements');">Wellbore &amp; Annuli</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblgrossperforations" class="grossperforations" style="cursor:hand" onclick="javascript:MDRTabClick('grossperforations');">Gross Perforations</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblwellhead" class="wellhead" style="cursor:hand" onclick="javascript:MDRTabClick('wellhead');">Well Head</span>
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

    <table width="100%">
      <tr>
        <td id="parentCell">


          <div id="holesection" stlye="width:100%">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Hole Section'])&gt;0">

                <div class="tableContainer">
                  <table id="tblHole" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Hole Section']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Hole Section']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblHole'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblHole&apos;);
          </script>

          <div id="casings">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Casings'])&gt;0">
                <table class="hidePrintLink">

                  <tr>
                    <td align="right">
                      <input id="chkParentCasings" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentCasings','chkChildCasings','ComponentInfo','WirelineRetrievables','tblCasings');"/>Component Info
                      &#160;
                      <input id="chkChildCasings" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentCasings','chkChildCasings','ComponentInfo','WirelineRetrievables','tblCasings');"/>Wireline Retrievables

                    </td>
                  </tr>
                </table>


                <div id="casings" class="tableContainer">
                  <table id="tblCasings"  class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">

                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>
                      </xsl:call-template>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblCasings'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblCasings&apos;);
            RemoveDuplicateRows(&apos;tblCasings&apos;);
          </script>
          <div id="liners">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Liners'])&gt;0">

                <table class="hidePrintLink" >

                  <tr>
                    <td align="right">
                      <input id="chkParentLiners" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentLiners','chkChildLiners','ComponentInfo','WirelineRetrievables','tblLiners');"/>Component Info
                      &#160;
                      <input id="chkChildLiners" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentLiners','chkChildLiners','ComponentInfo','WirelineRetrievables','tblLiners');"/>Wireline Retrievables

                    </td>
                  </tr>
                </table>
                <div id="liners" class="tableContainer">
                  <table id="tblLiners" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>
                      </xsl:call-template>
                    </tr>

                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>

                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblLiners'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblLiners&apos;);
            RemoveDuplicateRows(&apos;tblLiners&apos;);
          </script>

          <div id="mechanicalcontent">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Mechanical Content'])&gt;0">

                <div class="tableContainer">
                  <table id="tblMechanicalcontent" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Mechanical Content']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Mechanical Content']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>

                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblMechanicalcontent'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblMechanicalcontent&apos;);
          </script>
          <div id="fluidscements">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli'])&gt;0">

                <div class="tableContainer">
                  <table id="tblFluidscements" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblFluidscements'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblFluidscements&apos;);
          </script>
          <div id="grossperforations">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Gross Perforations'])&gt;0">
                <table class="hidePrintLink">

                  <tr>
                    <td>
                      <input id="chkParentGrossperforations" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentGrossperforations','chkChildGrossperforations','ComponentInfo','WirelineRetrievables','tblGrossperforations');"/>Perforation Intervals
                      &#160;
                      <input id="chkChildGrossperforations" type="checkbox" style="display:none" onclick="javascript:ShowHideColumnGroups('chkParentGrossperforations','chkChildGrossperforations','ComponentInfo','WirelineRetrievables','tblGrossperforations');"/><span style="display:none">NetPerforations</span>
                    </td>
                  </tr>
                </table>
                <div id="grossperforations" class="tableContainer">
                  <table id="tblGrossperforations" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>
                      </xsl:call-template>
                    </tr>

                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblGrossperforations'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblGrossperforations&apos;);
            RemoveDuplicateRows(&apos;tblGrossperforations&apos;);
          </script>
          <div id="wellhead">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellhead'])&gt;0">

                <div class="tableContainer">
                  <table id="tblWellhead" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellhead']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellhead']"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message">
                  <xsl:with-param name="tableId" select="'tblWellhead'"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblWellhead&apos;);
          </script>
        </td>
      </tr>
    </table>
    <script type="text/javascript">
      MDRTabClick(&apos;<xsl:value-of select="$activeDiv"/>&apos;);
      AddLastSelectedValue();
    </script>
  </xsl:template>

  <xsl:template name="CreateDataRows">
    <xsl:param name="childColl"/>
    <xsl:variable name="group" select="$childColl/@name"/>
    <xsl:for-each select="$childColl/record">
      <xsl:if test="count(hierarchy/record)&gt;0">
        <xsl:call-template name="CreateDataRows">
          <xsl:with-param name="childColl" select="hierarchy"/>
        </xsl:call-template>
      </xsl:if>
      <xsl:if test="count(hierarchy/record)=0">
        <tr>
          <xsl:if test="parent::*/@level='0'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='1'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='2'">
            <xsl:call-template name="AddCells">

              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='3'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*/parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="AddCells">
    <xsl:param name="record"/>
    <xsl:param name="group"/>
    <xsl:param name="level"/>
    <xsl:variable name="level1" select="'Strings'"/>
    <xsl:for-each select="$record/attribute[@display='true']">
      <td>
        <xsl:attribute name="group">
          <xsl:choose>
            <xsl:when test="$group='Casings'">
              <xsl:value-of select="$level1"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$group"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:if test="$level='2' or $level='3'">
          <xsl:attribute name="style">
            display:none
          </xsl:attribute>
        </xsl:if>
        <xsl:if test ="@type='number'">
          <xsl:attribute name="align">

            <xsl:value-of select="'right'"></xsl:value-of>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="string-length(@value)&gt;0">

          <xsl:variable name="refCol" select="@referencecolumn"/>
          <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>

          <xsl:choose>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($refUnit =$userPreference)">
              <xsl:value-of select="format-number(@value, '#0.00')"/>
            </xsl:when>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($userPreference = 'metres')">
              <xsl:choose>
                <xsl:when test="$refUnit = 'inches'">
                  <xsl:value-of select="format-number((@value * 25.4), '#0.00')"/>
                </xsl:when>
                <xsl:when test="$refUnit = 'mm'">
                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:when>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($userPreference = 'feet')">
              <xsl:choose>
                <xsl:when test="$refUnit = 'mm'">
                  <xsl:value-of select="format-number((@value div 25.4), '#0.00')"/>
                </xsl:when>
                <xsl:when test="$refUnit = 'inches'">
                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:when>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($userPreference = 'metres')">
              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
            </xsl:when>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($userPreference = 'feet')">
              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
            </xsl:when>
            <xsl:when test ="@type='number'">
              <xsl:value-of select="format-number(@value, '#0.00')"/>
            </xsl:when>
            <xsl:when test="@type = 'date'">
              <xsl:value-of select="objDate:GetDateTime(@value)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@value"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
        <xsl:if test="string-length(@value)=0">
          &#160;
        </xsl:if>
      </td>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="CreateHeaderRow">
    <xsl:param name="childColl"/>
    <xsl:param name="isData"/>
    <xsl:param name="isTopRow"/>
    <xsl:variable name="level1" select="'Strings'"/>
    <xsl:variable name="level2" select="'ComponentInfo'"/>
    <xsl:variable name="level3" select="'WirelineRetrievables'"/>
    <xsl:for-each select="$childColl/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px;" nowrap="nowrap">
        <xsl:attribute name="group">
          <xsl:value-of select="$level1"/>
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">
            <xsl:if test="position()=count($childColl/record[1]/attribute[@display='true'])">
              <xsl:attribute name="style">
                border-right:1px solid #bdbdbd;font-weight:bold"
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="position()='1'">
              <xsl:value-of select="$childColl/@name"></xsl:value-of>
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="class">
              Header
            </xsl:attribute>
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(m)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(ft)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px; " nowrap="nowrap">
        <xsl:attribute name="group">
          <xsl:value-of select="$level2"/>
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="style">
          display:none;border-bottom:1px solid #bdbdbd;font-weight:bold"
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">
            <xsl:if test="position()=count($childColl/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])">
              <xsl:attribute name="style">
                border-right:1px solid #bdbdbd;font-weight:bold;border-bottom:1px solid #bdbdbd;font-weight:bold"
              </xsl:attribute>
            </xsl:if>

            <xsl:if test="position()='1'">
              &#160;
              <xsl:value-of select="$childColl/record[hierarchy/@name][1]/hierarchy/@name"></xsl:value-of>
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="class">
              Header
            </xsl:attribute>
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(m)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(ft)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name]/hierarchy/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px;" nowrap="nowrap">
        <xsl:attribute name="group">
          <xsl:value-of select="$level3"/>
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="style">
          display:none;border-bottom:1px solid #bdbdbd;font-weight:bold"
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">
            <xsl:if test="position()='1'">
              &#160;
              <xsl:text>Wireline Retrievables</xsl:text>
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="class">
              Header
            </xsl:attribute>
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(mm)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(m)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$refUnit = 'inches'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:when test="$refUnit = 'mm'">
                      &#160;(inches)
                    </xsl:when>
                    <xsl:otherwise>
                      &#160;(ft)
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="Message">
    <xsl:param name="tableId" />
    <div>
      <table>
        <xsl:attribute name="id">
          <xsl:value-of select="$tableId" />
        </xsl:attribute>
        <tr>
          <td>
            <xsl:text>There is no item to show in this view.</xsl:text>
          </td>
        </tr>
      </table>
    </div>
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

  <xsl:template name="AddDataType">
    <xsl:param name="currentNode"/>
    <xsl:attribute name="type">
      <xsl:value-of select="$currentNode/@type"></xsl:value-of>
    </xsl:attribute>
  </xsl:template>

</xsl:stylesheet>