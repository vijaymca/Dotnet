﻿<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt2="urn:frontpage:internal" xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <!--Parameter declaration start -->

  <!--Pagination parameter declaration start here-->
  <xsl:param name="recordsPerPage" select="0" />
  <xsl:param name="maxPageLinkToDisplay" select="0" />
  <xsl:param name="pageCount" select="0" />
  <xsl:param name="recordCount" select="0" />
  <xsl:param name="startPageNumber" select="0" />
  <xsl:param name="endPageNumber" select="0" />
  <xsl:param name="currentPageNumber" select="1" />
  <xsl:param name="recordStarIndex" select="0" />
  <xsl:param name="recordEndIndex" select="0" />
  <xsl:param name="searchType" select="''" />
  <!--Pagination parameter declaration ends here-->

  <!--Display Unit Value-->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:param name="pressureUnit" select="''"/>
  <xsl:param name="temperatureUnit" select="''"/>
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="activeDiv"/>
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>
  <!--Selecting checkboxes-->
  <xsl:param name ="arrRowSelectedCheckboxes" select="''"/>
  <xsl:param name ="arrColSelectedCheckboxes" select="''"/>
  <!--Parameter declaration end -->

  <!--Template to create the Response table.-->
  <xsl:template match="/" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">
    <!--table to display response data.-->
    <div class="gray_embossed_tabs_r" >
      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
          <td>
            <ul class="tabNavigation">
              <li>
                <a>
                  <span id="lblPressureSurvey" class="PressureSurvey" style="cursor:hand;" onclick="javascript:MDRTabClick('PressureSurvey');"> Pressure Survey Data</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lblReservoir" class="Reservoir" style="cursor:hand" onclick="javascript:MDRTabClick('Reservoir');">  Reservoir Data</span>
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
          <div id="PressureSurvey" stlye="width:100%">
            <xsl:choose>
              <xsl:when test="count(response/report[@name='PressureSurvey']/record)&gt;0">
                <table class="hidePrintLink">
                  <tr>
                    <td>
                      <xsl:call-template name="Paging">
                        <xsl:with-param name="divName" select="'PressureSurvey'"/>
                        <xsl:with-param name="reportName" select="'PressureSurvey'"/>
                      </xsl:call-template>
                    </td>
                  </tr>
                </table>
                <div class="tableContainer">
                  <table id="tblPressureSurvey" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <thead class="fixedHeader" id="fixedHeader">
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="childColl" select="response/report[@name='PressureSurvey']/record"/>
                        <xsl:with-param name ="tableId" select="'tblPressureSurvey'"/>
                      </xsl:call-template>
                    </thead>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="AddDataRows">
                        <xsl:with-param name="currentDiv" select="'PressureSurvey'"/>
                        <xsl:with-param name="childColl" select="response/report[@name='PressureSurvey']/record"/>
                        <xsl:with-param name ="tableId" select="'tblPressureSurvey'"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
              </xsl:when>

              <xsl:otherwise>
                <xsl:call-template name="Message"></xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </div>
          <div id="Reservoir" stlye="width:100%">
            <xsl:choose>
              <xsl:when test="count(response/report[@name='Reservoir']/record)&gt;0">
                <table class="hidePrintLink" >
                  <tr>
                    <td>
                      <xsl:call-template name="Paging">
                        <xsl:with-param name="divName" select="'Reservoir'"/>
                        <xsl:with-param name="reportName" select="'Reservoir'"/>
                      </xsl:call-template>
                    </td>
                  </tr>
                </table>
                <div class="tableContainer">
                  <table id="tblReservoir" class="scrollTable" style="border-right:1px solid #bdbdbd;width:100%" cellpadding="0" cellspacing="0">
                    <thead class="fixedHeader" id="fixedHeader">
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="childColl" select="response/report[@name='Reservoir']/record"/>
                        <xsl:with-param name ="tableId" select="'tblReservoir'"/>
                      </xsl:call-template>
                    </thead>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="AddDataRows">
                        <xsl:with-param name="currentDiv" select="'Reservoir'"/>
                        <xsl:with-param name="childColl" select="response/report[@name='Reservoir']/record"/>
                        <xsl:with-param name ="tableId" select="'tblReservoir'"/>
                      </xsl:call-template>
                    </tbody>
                  </table>
                </div>
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
      MDRTabClick(&apos;<xsl:value-of select="$activeDiv"/>&apos;);
    </script>
  </xsl:template>

  <xsl:template name="Message">
    <xsl:text>There is no item to show in this view.</xsl:text>
  </xsl:template>

  <xsl:template name="CreateHeaderRow">
    <xsl:param name="childColl"/>
    <xsl:param name ="tableId"/>
    <xsl:call-template name="CreateCheckBoxRow">
      <xsl:with-param name="childColl" select="$childColl"/>
      <xsl:with-param name ="tableId" select="$tableId"/>
    </xsl:call-template>

    <tr>
      <th id="hidePrintCol" width="15px" text-align="center">
        <xsl:element name="input">
          <xsl:attribute name="type">checkbox</xsl:attribute>
          <xsl:attribute name="id">chkbxRowSelectAll</xsl:attribute>
          <xsl:attribute name="value">
            <xsl:value-of select="$childColl[1]/attribute[@contextkey='true']/@name"/>
          </xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:text disable-output-escaping="yes">
                SelectAllCheckBoxOnChange(this,'
            </xsl:text>
            <xsl:value-of select ="$tableId"/>
            <xsl:text disable-output-escaping="yes">
                ','tbody','chkbxRow','hidRowSelectedCheckBoxes');
            </xsl:text>
          </xsl:attribute>
        </xsl:element>
      </th>
      <xsl:for-each select="$childColl[1]/attribute[@display='true']">
        <th>
          <xsl:call-template name="AddDataType">
            <xsl:with-param name="currentNode" select="."></xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="ApplyToolTip">
            <xsl:with-param name="currentNode" select="."></xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="HideColumn">
          </xsl:call-template>
          <xsl:choose>
            <xsl:when test="string(@referencecolumn)">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value" />
              <xsl:value-of select="@name"/>&#160;
              <xsl:choose>
                <xsl:when test="($refUnit = 'metres') or ($refUnit = 'feet')">
                  <xsl:call-template name="RenderFeetMeterLabel"/>
                </xsl:when>
                <xsl:when test="($refUnit = 'degc') or ($refUnit = 'degf') or ($refUnit = 'degC') or ($refUnit = 'degF')">
                  <xsl:call-template name="RenderTemperatureUnitLabel"/>
                </xsl:when>
                <xsl:when test="($refUnit = 'bara') or ($refUnit = 'kpa') or ($refUnit = 'psia')">
                  <xsl:call-template name="RenderPressureUnitLabel"/>
                </xsl:when>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@name"/>&#160;
            </xsl:otherwise>
          </xsl:choose>
          <xsl:call-template name="ApplySort">
            <xsl:with-param name="currentNode" select="."/>
          </xsl:call-template>
        </th>
      </xsl:for-each>
    </tr>
  </xsl:template>

  <xsl:template name="CreateCheckBoxRow">
    <xsl:param name="childColl"/>
    <xsl:param name ="tableId"/>
    <tr id="hidePrintRow">
      <th id="hidePrintCol" width="15px" text-align="center">
        <xsl:element name="input">
          <xsl:attribute name="type">checkbox</xsl:attribute>
          <xsl:attribute name="id">chkbxColumnSelectAll</xsl:attribute>
          <xsl:attribute name="value">SelectAllColumn</xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:text disable-output-escaping="yes">
                SelectAllCheckBoxOnChange(this,'
            </xsl:text>
            <xsl:value-of select ="$tableId"/>
            <xsl:text disable-output-escaping="yes">
                ','thead','chkbxColumn','hidColSelectedCheckBoxes');
            </xsl:text>
          </xsl:attribute>
        </xsl:element>
      </th>

      <xsl:for-each select="$childColl[1]/attribute[@display='true']">

        <th id="hidePrintCol" width="15px" align="center"  text-align="center">
          <xsl:call-template name="HideColumn"/>
          <xsl:element name="input">
            <xsl:attribute name="type">checkbox</xsl:attribute>
            <xsl:attribute name="id">chkbxColumn</xsl:attribute>
            <xsl:attribute name="value">
              <xsl:value-of select="@name"/>
              <xsl:value-of select="'|'"/>
            </xsl:attribute>
            <xsl:attribute name="onclick">
              <xsl:text disable-output-escaping="yes">
                CheckBoxOnChange(this,'
            </xsl:text>
              <xsl:value-of select ="$tableId"/>
              <xsl:text disable-output-escaping="yes">
                ','thead','chkbxColumnSelectAll','hidColSelectedCheckBoxes');
            </xsl:text>
            </xsl:attribute>
            <xsl:call-template name="String-Equals">
              <xsl:with-param name ="arrString" select="$arrColSelectedCheckboxes"/>
              <xsl:with-param name="stringToCompare" select="@name"/>
              <xsl:with-param name="seperator" select="'|'"/>
            </xsl:call-template>
          </xsl:element>
        </th>
      </xsl:for-each>
    </tr>
  </xsl:template>

  <xsl:template name="AddDataRows">
    <xsl:param name="childColl"/>
    <xsl:param name="currentDiv"/>
    <xsl:param name ="tableId"/>
    <xsl:for-each select="$childColl">
      <xsl:choose>
        <xsl:when test="$activeDiv=$currentDiv">
          <xsl:if test="(position()&gt;=$recordStarIndex)and(position()&lt;=$recordEndIndex)">
            <xsl:call-template name="CreateRow">
              <xsl:with-param name="currentNode" select="."/>
              <xsl:with-param name ="tableId" select="$tableId"/>
            </xsl:call-template>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:if test="(position()&gt;=1)and(position()&lt;=$recordsPerPage)">
            <xsl:call-template name="CreateRow">
              <xsl:with-param name="currentNode" select="."/>
              <xsl:with-param name ="tableId" select="$tableId"/>
            </xsl:call-template>
          </xsl:if>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="CreateRow">
    <xsl:param name="currentNode"></xsl:param>
    <xsl:param name ="tableId"/>
    <tr>
      <td id="hidePrintCol" width="15px" text-align="center">
        <xsl:element name="input">
          <xsl:variable name="value" select="$currentNode/attribute[@contextkey='true']/@name"/>
          <xsl:attribute name="type">checkbox</xsl:attribute>
          <xsl:attribute name="id">chkbxRow</xsl:attribute>
          <xsl:attribute name="value">
            <xsl:value-of select="$value"/>
            <xsl:value-of select="'|'"/>
          </xsl:attribute>
          <xsl:attribute name="onclick">
            <xsl:text disable-output-escaping="yes">
                CheckBoxOnChange(this,'
            </xsl:text>
            <xsl:value-of select ="$tableId"/>
            <xsl:text disable-output-escaping="yes">
                ','tbody','chkbxRowSelectAll','hidRowSelectedCheckBoxes');
            </xsl:text>
          </xsl:attribute>
          <xsl:if test="contains($arrRowSelectedCheckboxes,$value)">
            <xsl:attribute name="checked">checked</xsl:attribute>
          </xsl:if>
        </xsl:element>
      </td>
      <xsl:call-template name="AddCells">
        <xsl:with-param name="record" select="$currentNode"/>
      </xsl:call-template>
    </tr>
  </xsl:template>

  <xsl:template name="AddCells">
    <xsl:param name="record"/>
    <xsl:for-each select="$record/attribute[@display='true']">

      <xsl:choose>
        <xsl:when test="string(@value)">
          <xsl:element name="td">
            <xsl:call-template name="HideColumn">
            </xsl:call-template>
            <xsl:variable name="refCol" select="@referencecolumn"/>
            <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value" />
            <xsl:choose>
              <xsl:when test="(string-length(@referencecolumn)&gt;0)">
                <xsl:attribute name="style">
                  <xsl:value-of select="'text-align:right'"/>
                </xsl:attribute>
                <xsl:choose>
                  <xsl:when test="@value = 'No data'">
                    <xsl:value-of select="@value"/>
                  </xsl:when>
                  <xsl:when test="($refUnit = 'metres') or ($refUnit ='feet')">
                    <xsl:call-template name="FeetMeterConvertor">
                      <xsl:with-param name="refUnit" select="$refUnit" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:when test="($refUnit = 'degc') or ($refUnit = 'degf') or ($refUnit = 'degC') or ($refUnit = 'degF')">
                    <xsl:call-template name="TemperatureUnitConvertor">
                      <xsl:with-param name="refUnit" select="$refUnit" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:when test="($refUnit = 'bara') or ($refUnit = 'kpa') or ($refUnit = 'psia')">
                    <xsl:call-template name="PressureUnitConvertor">
                      <xsl:with-param name="refUnit" select="$refUnit" />
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number(@value, '#0.00')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test ="@type='number'">
                <xsl:attribute name="style">
                  <xsl:value-of select="'text-align:right'"/>
                </xsl:attribute>
                <xsl:choose>
                  <xsl:when test="@value = 'No data'">
                    <xsl:value-of select="@value"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number(@value, '#0.00')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="@type = 'date'">
                <xsl:value-of select="objDate:GetDateTime(@value)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:call-template name="AddTVDSSCell">
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:element>
        </xsl:when>
        <xsl:otherwise>
          <td>
            <xsl:call-template name="HideColumn">
            </xsl:call-template>
            &#160;
          </td>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="Paging">
    <xsl:param name="divName"></xsl:param>
    <xsl:param name="reportName"></xsl:param>
    <xsl:variable name="totalRecord" select="count(response/report[@name=$reportName]/record)"></xsl:variable>
    <xsl:variable name="Pages" select="ceiling($totalRecord div $recordsPerPage)" />
    <!--for paging     -->
    <!-- Call Templates of for page number Loop-->
    <xsl:choose>
      <xsl:when test="$activeDiv=$divName">
        <xsl:call-template name="CreatePagingLink">
          <xsl:with-param name="paramStartPageNumber" select="$startPageNumber"/>
          <xsl:with-param name="paramEndPageNumber" select="$endPageNumber"/>
          <xsl:with-param name="paramRecordCount" select="$totalRecord"/>
          <xsl:with-param name="paramCurrentPageNumber" select="$currentPageNumber"/>
          <xsl:with-param name="paramPageCount" select="$Pages"/>
        </xsl:call-template>
        <br></br>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="CreatePagingLink">
          <xsl:with-param name="paramStartPageNumber" select="1"/>
          <xsl:with-param name="paramEndPageNumber" select="$maxPageLinkToDisplay"/>
          <xsl:with-param name="paramRecordCount" select="$totalRecord"/>
          <xsl:with-param name="paramCurrentPageNumber" select="1"/>
          <xsl:with-param name="paramPageCount" select="$Pages"/>
        </xsl:call-template>
        <br></br>
      </xsl:otherwise>
    </xsl:choose>
    <!-- End Call Templates of for page number Loop-->
    <!--end paging     -->
  </xsl:template>

  <xsl:template name="AddTVDSSCell">
    <xsl:choose>
      <xsl:when test="@name ='Depth References'">
        <xsl:value-of select="@value"/><xsl:value-of select="@default"/>|<xsl:value-of select="@unit"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@value"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--Create Paging Template-->
  <!-- Template to create page link start-->
  <xsl:template name ="CreatePagingLink">
    <xsl:param name="paramStartPageNumber"/>
    <xsl:param name="paramEndPageNumber"/>
    <xsl:param name="paramRecordCount"/>
    <xsl:param name="paramCurrentPageNumber"/>
    <xsl:param name="paramPageCount"/>
    <table>
      <tr>
        <td align="top">
          <xsl:if test="$paramCurrentPageNumber != 1">
            <!-- <xsl:call-template name="CreatePageHyperlink">
        <xsl:with-param name="linkNumber" select="1"/>
     <xsl:with-param name="linkText" select="'First'"/>
      </xsl:call-template>-->
            <xsl:call-template name="CreatePageHyperlink">
              <xsl:with-param name="linkNumber" select="$paramCurrentPageNumber -1"/>
              <xsl:with-param name="linkText" select="'Previous'"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:call-template name="CreateLink" >
            <xsl:with-param name="startPageNumber" select="$paramStartPageNumber"/>
            <xsl:with-param name="endPageNumber" select="$paramEndPageNumber"/>
            <xsl:with-param name="currentPageNumber" select="$paramCurrentPageNumber"/>
          </xsl:call-template>
          <xsl:if test="$paramCurrentPageNumber != $paramPageCount">
            <xsl:call-template name="CreatePageHyperlink">
              <xsl:with-param name="linkNumber" select="$paramCurrentPageNumber + 1"/>
              <xsl:with-param name="linkText" select="'Next'"/>
            </xsl:call-template>
            <!-- <xsl:call-template name="CreatePageHyperlink">
        <xsl:with-param name="linkNumber" select="$pageCount"/>
        <xsl:with-param name="linkText" select="'Last'"/>
      </xsl:call-template>-->
          </xsl:if>
        </td>
      </tr>
    </table>
    <br/>
  </xsl:template>
  <xsl:template name ="CreateLink">
    <xsl:param name="startPageNumber"/>
    <xsl:param name="endPageNumber"/>
    <xsl:param name="currentPageNumber"/>
    <xsl:if test="$startPageNumber &lt; $endPageNumber">
      <xsl:choose>
        <xsl:when test="($startPageNumber+1) = $currentPageNumber">
          &#160;
          <b>
            [ <xsl:value-of select="($startPageNumber+1)"/> ]
          </b>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="CreatePageHyperlink">
            <xsl:with-param name="linkNumber" select="$startPageNumber + 1"/>
            <xsl:with-param name="linkText" select="$startPageNumber + 1"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="CreateLink">
        <xsl:with-param name="startPageNumber" select="$startPageNumber+1"/>
        <xsl:with-param name="endPageNumber" select="$endPageNumber"/>
        <xsl:with-param name="currentPageNumber" select="$currentPageNumber"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name ="CreatePageHyperlink">
    <xsl:param name="linkNumber"/>
    <xsl:param name="linkText"/>
    <xsl:text disable-output-escaping="yes">&#160;&lt;a href="Javascript:OnTabTabularPageLinkClick(</xsl:text>
    <xsl:value-of select="$linkNumber"/>,
    <xsl:choose>
      <xsl:when test="string($sortBy)">
        '<xsl:value-of select="$sortBy"/>',
      </xsl:when>
      <xsl:otherwise>
        '',
      </xsl:otherwise>
    </xsl:choose>
    '<xsl:value-of select="$sortType"/>',this.parentElement.parentElement.parentElement.parentElement.parentElement.id)"
    <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
    <xsl:value-of select="$linkText"/>
    <xsl:text disable-output-escaping="yes">&lt;/a &gt;</xsl:text>
  </xsl:template>
  <!-- Template to create page link end-->

  <xsl:template name="HideColumn">
    <xsl:choose>
      <xsl:when test="@name ='Depth References'">
        <xsl:attribute name="style">
          display:none
        </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="ApplySort">
    <xsl:param name="currentNode"/>

    <xsl:choose>
      <xsl:when test="$sortBy = $currentNode/@name">
        <xsl:choose>
          <xsl:when test="$sortType='ascending'">
            <xsl:call-template name="AddSortLink">
              <xsl:with-param name="currentNode" select="$currentNode"/>
              <xsl:with-param name="imgName" select="'UP_ACTIVE.gif'"/>
              <xsl:with-param name="sortOrder" select="'descending'"/>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="AddSortLink">
              <xsl:with-param name="currentNode" select="$currentNode"/>
              <xsl:with-param name="imgName" select="'DOWN_ACTIVE.gif'"/>
              <xsl:with-param name="sortOrder" select="'ascending'"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="AddSortLink">
          <xsl:with-param name="currentNode" select="$currentNode"/>
          <xsl:with-param name="imgName" select="'UP_INACTIVE.gif'"/>
          <xsl:with-param name="sortOrder" select="'descending'"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="AddSortLink">
    <xsl:param name="currentNode"/>
    <xsl:param name="imgName"/>
    <xsl:param name="sortOrder"/>
    <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="#" onclick="Javascript:OnTabTabularSortLinkClick(</xsl:text>
    '<xsl:value-of select="$currentNode/@name"/>','<xsl:value-of select="$sortOrder"/>',
    this.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement
    <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
    <img class="hidePrintLink" alt="click to sort">
      <xsl:attribute name ="src">
        <xsl:value-of select ="concat('/_layouts/DREAM/images/',$imgName)"></xsl:value-of>
      </xsl:attribute>
    </img>
    <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>

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

  <xsl:template name="FeetMeterConvertor">
    <xsl:param name="refUnit" ></xsl:param>
    <xsl:choose>
      <xsl:when test="$refUnit = $userPreference">
        <xsl:value-of select="format-number(@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="$userPreference = 'metres'">
        <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
      </xsl:when>
      <xsl:when test="$userPreference = 'feet'">
        <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="TemperatureUnitConvertor">
    <xsl:param name="refUnit" />
    <xsl:choose>
      <xsl:when test="$refUnit = $temperatureUnit">
        <xsl:value-of select="format-number(@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degc') or ($temperatureUnit = 'degC')">
        <!-- c=(f-32)/1.8-->
        <xsl:value-of select="format-number((((@value) - 32) div 1.8), '#0.00')"/>
        <!--<xsl:value-of select="format-number(($currentAttribute/@value div 33.8), '#0.00')"/>-->
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degf') or ($temperatureUnit = 'degF')">
        <!-- f=(c*1.8)+32-->
        <xsl:value-of select="format-number((((@value) * 1.8) + 32), '#0.00')"/>
        <!--<xsl:value-of select="format-number(($currentAttribute/@value * 33.8), '#0.00')"/>-->
      </xsl:when>
      <!--<xsl:when test="($temperatureUnit = 'degc') or ($temperatureUnit = 'degC')">
        <xsl:value-of select="format-number((@value div 33.8), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degf') or ($temperatureUnit = 'degF')">
        <xsl:value-of select="format-number((@value * 33.8), '#0.00')"/>
      </xsl:when>-->
    </xsl:choose>
  </xsl:template>

  <xsl:template name="PressureUnitConvertor">
    <xsl:param name="refUnit" />
    <xsl:choose>
      <xsl:when test="$refUnit = $pressureUnit">
        <xsl:value-of select="format-number(@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'bara') and ($pressureUnit = 'kpa')">
        <xsl:value-of select="format-number((@value * 100), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'bara') and ($pressureUnit = 'psia')">
        <xsl:value-of select="format-number((@value * 14.503774), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'kpa') and ($pressureUnit = 'bara')">
        <xsl:value-of select="format-number((@value * 0.01), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'kpa') and ($pressureUnit = 'psia')">
        <xsl:value-of select="format-number((@value * 0.145038), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'psia') and ($pressureUnit = 'bara')">
        <xsl:value-of select="format-number((@value * 0.068948), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'psia') and ($pressureUnit = 'kpa')">
        <xsl:value-of select="format-number((@value * 6.894745), '#0.00')"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="RenderFeetMeterLabel">
    <xsl:choose>
      <xsl:when test="$userPreference = 'metres'">
        (m)&#160;
      </xsl:when>
      <xsl:otherwise>
        (ft)&#160;
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="RenderTemperatureUnitLabel">
    <xsl:choose>
      <xsl:when test="($temperatureUnit = 'degc') or ($temperatureUnit = 'degC')">
        (degC)&#160;
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degf') or ($temperatureUnit = 'degF')">
        (degF)&#160;
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="RenderPressureUnitLabel">
    <xsl:choose>
      <xsl:when test="$pressureUnit = 'bara'">
        (barA)&#160;
      </xsl:when>
      <xsl:when test="$pressureUnit = 'kpa'">
        (kPa)&#160;
      </xsl:when>
      <xsl:when test="$pressureUnit = 'psia'">
        (psiA)&#160;
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!--This templates is used to compare column names in column names array,
  since one cloumn can contain another so checking for equals.ex Wellbore name is contained in Parent Wellbore Name-->
  <xsl:template name="String-Equals">
    <xsl:param name="arrString" />
    <xsl:param name="stringToCompare" />
    <xsl:param name="seperator" />
    <xsl:variable name="first" select="substring-before($arrString,$seperator)" />
    <xsl:variable name="remaining" select="substring-after($arrString,$seperator)" />
    <xsl:choose>
      <xsl:when test="$first = $stringToCompare">
        <xsl:attribute name="checked">checked</xsl:attribute>
      </xsl:when>
      <xsl:when test="$remaining">
        <xsl:call-template name="String-Equals">
          <xsl:with-param name="stringToCompare" select="$stringToCompare" />
          <xsl:with-param name="arrString" select="$remaining" />
          <xsl:with-param name="seperator" select="$seperator" />
        </xsl:call-template>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
