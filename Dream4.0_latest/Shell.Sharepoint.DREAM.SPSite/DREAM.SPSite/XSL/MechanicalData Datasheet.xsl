<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt2="urn:frontpage:internal" xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <!--Parameter declaration start -->
  <!--Display Unit Value-->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:variable name="unitValue" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>
  <!--Parameter declaration end -->
  <!--Template to create the Response table.-->

  <xsl:template match="/" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">
    <!--table to display response data.-->
    <!--table to display Well header data.-->
    <table id="tblHeader" style="border-right:1px solid #bdbdbd" cellpadding="0" cellspacing="0">
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

    <div class="gray_embossed_tabs_r" >
      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
          <td>
            <ul class="tabNavigation">
              <li>
                <a>
                  <span id="lblHolesection" style="cursor:hand;" onclick="javascript:TabClick('holesection','lblHolesection');">Hole Sections</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblCasings" style="cursor:hand" onclick="javascript:TabClick('casings','lblCasings');">Casings</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblLiners" style="cursor:hand" onclick="javascript:TabClick('liners','lblLiners');">Liners</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblMechanicalcontent" style="cursor:hand" onclick="javascript:TabClick('mechanicalcontent','lblMechanicalcontent');">Mechanical Content</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblFluidscements" style="cursor:hand" onclick="javascript:TabClick('fluidscements','lblFluidscements');">Wellbore &amp; Annuli</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblGrossperforations" style="cursor:hand" onclick="javascript:TabClick('grossperforations','lblGrossperforations');">Gross Perforations</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblWellhead" style="cursor:hand" onclick="javascript:TabClick('wellhead','lblWellhead');">Well Head</span>
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

    <table class="tableAddEvents" id="tblMain" width="100%">
      <tr>
        <td id="parentCell">
          <div id="holesection">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Hole Section'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Hole Section']"/>
                  <xsl:with-param name="tableId" select="'tblholesection'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="casings">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Casings'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>
                  <xsl:with-param name="tableId" select="'tblCasings'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="liners">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Liners'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>
                  <xsl:with-param name="tableId" select="'tblliners'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="mechanicalcontent">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Mechanical Content'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Mechanical Content']"/>
                  <xsl:with-param name="tableId" select="'tblmechanicalcontent'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="fluidscements">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli']"/>
                  <xsl:with-param name="tableId" select="'tblWellboreAnnuli'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="grossperforations">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Gross Perforations'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>
                  <xsl:with-param name="tableId" select="'tblgrossperforations'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>

          <div id="wellhead">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellhead'])&gt;0">
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellhead']"/>
                  <xsl:with-param name="tableId" select="'tblwellhead'"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
        </td>
      </tr>
    </table>
    <script type="text/javascript">
      TabClick(&apos;holesection&apos;,&apos;lblHolesection&apos;);
      <!--ChkClick(&apos;tableContainer&apos;,&apos;chkComp&apos;,&apos;chkWire&apos;,&apos;general&apos;)-->
      AddLastSelectedValue();
    </script>
  </xsl:template>


  <xsl:template name="GenerateHierarchy">
    <xsl:param name="childColl"/>
    <xsl:param name="tableId"></xsl:param>
    <xsl:if test="$childColl/@level !='1'">
      <b>
        <xsl:value-of select="$childColl/@name"></xsl:value-of>
      </b>
    </xsl:if>
    <table class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
      <xsl:attribute name="id">
        <xsl:value-of select="$tableId"/>
      </xsl:attribute>
      <tr>
        <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
          &#160;
        </th>
        <xsl:for-each select="$childColl/record[1]/attribute[@display='true'][@title='true']">
          <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
            <xsl:call-template name="ApplyToolTip">
              <xsl:with-param name="currentNode" select="."></xsl:with-param>
            </xsl:call-template>
            <xsl:value-of select="@name"/>
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
          </th>
        </xsl:for-each>
        <xsl:for-each select="$childColl/record[1]/attribute[@display='true'][@title='false']">
          <th style="width:auto;border-top:1px solid #bdbdbd;font-weight:lighter;" class="Header">
            <xsl:call-template name="ApplyToolTip">
              <xsl:with-param name="currentNode" select="."></xsl:with-param>
            </xsl:call-template>
            <xsl:value-of select="@name"/>
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
          </th>
        </xsl:for-each>
        <xsl:for-each select="$childColl/record[1]/attribute[@display='false']">
          <th style="display:none">
            <xsl:call-template name="ApplyToolTip">
              <xsl:with-param name="currentNode" select="."></xsl:with-param>
            </xsl:call-template>
            <xsl:value-of select="@name"/>
          </th>
        </xsl:for-each>
      </tr>

      <tbody class="scrollContent">
        <xsl:for-each select="$childColl/record">
          <xsl:variable name="childCount" select="count(hierarchy/record)"/>
          <xsl:variable name="attrCount" select="count(attribute[@display='true'])+1"/>
          <tr>
            <td style="width:auto">
              <xsl:if test="$childCount&gt;0">
                <img src="/_layouts/dream/images/plus.gif" alt="Expand" onclick="MDRExpandCollapse(this);"/>
              </xsl:if>
              <xsl:if test="$childCount=0">
                &#160;
              </xsl:if>
            </td>
            <xsl:for-each select="attribute[@display='true'][@title='true']">
              <td style="width:auto;">
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
            <xsl:for-each select="attribute[@display='true'][@title='false']">
              <td style="width:auto;">
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
            <xsl:for-each select="attribute[@display='false']">
              <td style="width:auto;display:none">
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
          </tr>
          <xsl:if test="$childCount&gt;0">
            <tr style="display:none">
              <td style="width:auto">
                &#160;
              </td>
              <td style="width:auto;padding:6px">
                <xsl:attribute name="colspan">
                  <xsl:value-of select="$attrCount"/>
                </xsl:attribute>
                <xsl:call-template name="GenerateHierarchy">
                  <xsl:with-param name="childColl" select="hierarchy"/>
                  <xsl:with-param name="tableId" select="'tblhierarchy'"/>
                </xsl:call-template>
              </td>
            </tr>          
          </xsl:if>
        </xsl:for-each>
      </tbody>
    </table>
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
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='1'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='2'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='3'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*/parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/parent::*/parent::*/@name"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
            </xsl:call-template>
          </xsl:if>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="AddCells">
    <xsl:param name="record"/>
    <xsl:param name="group"/>
    <xsl:for-each select="$record/attribute[@display='true']">
      <td>
        <xsl:attribute name="group">
          <xsl:value-of select="$group"/>
        </xsl:attribute>
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

    <xsl:variable name="level1" select="'Strings'"/>
    <xsl:variable name="level2" select="'ComponentInfo'"/>
    <xsl:variable name="level3" select="'WirelineRetrievables'"/>
    <xsl:for-each select="$childColl/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px;" nowrap="nowrap"  class="Header">
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="group">
          <xsl:value-of select="$level1"/>
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
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
      </th>
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px;" nowrap="nowrap"  class="Header">
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="group">
          <xsl:value-of select="$level2"/>
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
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

      </th>
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name]/hierarchy/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #bdbdbd;font-weight:bold;padding:4px;" nowrap="nowrap"  class="Header">
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="group">
          <xsl:value-of select="$level3"/>
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
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

      </th>
    </xsl:for-each>
  </xsl:template>


  <xsl:template name="Message">
    <xsl:text>There is no item to show in this view.</xsl:text>
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