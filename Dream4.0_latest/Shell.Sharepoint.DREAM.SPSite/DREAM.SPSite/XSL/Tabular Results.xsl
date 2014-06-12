<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:objDate="urn:DATE">

  <xsl:output method="xml"/>

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
  <xsl:param name="userPreference" select="''"/>
  <xsl:param name="pressureUnit" select="''"/>
  <xsl:param name="temperatureUnit" select="''"/>
  <xsl:param name="arrColNames" select="''"/>
  <xsl:param name="arrColDisplayStatus" select="''" />
  <xsl:param name="reorderDivHTML" />
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>
  <!-- Data Quality Check-->
  <xsl:param name="LowRange"/>
  <xsl:param name="MediumRange"/>
  <xsl:param name="HighRange"/>
  <xsl:param name="LowRangeColor"/>
  <xsl:param name="MediumRangeColor"/>
  <xsl:param name="HighRangeColor"/>
  <!-- End Data Quality Check-->
  <!--Selecting checkboxes-->
  <xsl:param name ="arrRowSelectedCheckboxes" select="''"/>
  <xsl:param name ="arrColSelectedCheckboxes" select="''"/>
  <xsl:param name ="assetColName" select="''"/>
  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="(string-length($arrColNames)&gt;0) and (contains($arrColDisplayStatus,'true'))">

        <!-- Call Templates of for page number Loop-->
        <xsl:call-template name="CreatePagingLink"/>
        <!--Looping through each report-->
        <xsl:for-each select="response/report">
          <!--<xsl:call-template name="RenderRecordsInfo"/>-->
          <!-- End Of Show previous/next page links-->
          <!--Creating Scrollbar.-->
          <div id="tableContainer" class="tableContainer">
            <!--table to display response data.-->
            <table style="border-right:1px solid #BDBDBD;"  cellpadding="0" cellspacing="0" 
              id="tblSearchResults">
              <!-- Adding search name as an attribute to get name of search type for current page-->
              <xsl:attribute name="searchType">
                <xsl:value-of select="$searchType"/>
              </xsl:attribute>
              <!--Name of assetname col ex for Well asset 'Well Name' -->
              <xsl:attribute name="assetColName">
                <xsl:value-of select="$assetColName"/>
              </xsl:attribute>
              <!-- Adding col tag for showing hiding columns-->
              <!--col tag for charting link-->
              <xsl:if test="($searchType = 'directionalsurveydetail') or ($searchType = 'picksdetail')">
                <col class="show printHide" width="15px" text-align="center"></col>
                <!--for chart link column -->
              </xsl:if>
              <!--end of column for charting -->
              <col class="show printHide" width="15px" text-align="center"></col>
              <xsl:call-template name="string-tokenizer">
                <xsl:with-param name="templateName" select="'RenderColTag'" />
                <xsl:with-param name="arrstring" select="$arrColDisplayStatus" />
                <xsl:with-param name="seperator" select="'|'" />
              </xsl:call-template>
              <!--End of Adding col tag-->
              <xsl:for-each select="record[@recordno=1]">
                <thead class="fixedHeader" id="fixedHeader">
                  <tr id="hidePrintRow" style="height: 20px;" align="center">
                    <!--column for charting-->
                    <xsl:if test="($searchType = 'directionalsurveydetail') or ($searchType = 'picksdetail')">
                      <th class="checkLocked">
                        &#160;
                      </th>
                    </xsl:if>
                    <!--end of column for charting -->
                    <th class="checkLocked">
                      <xsl:element name="input">
                        <xsl:attribute name="type">checkbox</xsl:attribute>
                        <xsl:attribute name="id">chkbxColumnSelectAll</xsl:attribute>
                        <xsl:attribute name="value">SelectAllColumn</xsl:attribute>
                        <xsl:attribute name="onclick">
                          SelectAllCheckBoxOnChange(this,'tblSearchResults','thead','chkbxColumn','hidColSelectedCheckBoxes');
                        </xsl:attribute>
                      </xsl:element>
                    </th>
                    <xsl:call-template name="string-tokenizer">
                      <xsl:with-param name="templateName" select="'RenderThChkBx'" />
                      <xsl:with-param name="currentNode" select="." />
                      <xsl:with-param name="arrstring" select="$arrColNames" />
                      <xsl:with-param name="seperator" select="'|'" />
                    </xsl:call-template>
                  </tr >
                  <tr style="height: 20px;">
                    <!--column for charting-->
                    <xsl:if test="($searchType = 'directionalsurveydetail') or ($searchType = 'picksdetail')">
                      <th class="checkLocked">
                        &#160;
                      </th>
                    </xsl:if>
                    <!--end of column for charting -->

                    <th class="checkLocked">
                      <xsl:element name="input">
                        <xsl:attribute name="type">checkbox</xsl:attribute>
                        <xsl:attribute name="id">chkbxRowSelectAll</xsl:attribute>
                        <xsl:attribute name="value">
                          <xsl:value-of select="attribute[@contextkey='true'][1]/@name"/>
                        </xsl:attribute>
                        <xsl:attribute name="onclick">
                          SelectAllCheckBoxOnChange(this,'tblSearchResults','tbody','chkbxRow','hidRowSelectedCheckBoxes');
                        </xsl:attribute>
                      </xsl:element>
                      <xsl:call-template name="RenderReorderLink">
                      </xsl:call-template>
                    </th>
                    <xsl:call-template name="string-tokenizer">
                      <xsl:with-param name="templateName" select="'RenderThCell'" />
                      <xsl:with-param name="currentNode" select="." />
                      <xsl:with-param name="arrstring" select="$arrColNames" />
                      <xsl:with-param name="seperator" select="'|'" />
                    </xsl:call-template>
                  </tr>
                </thead>
              </xsl:for-each>
              <!--Looping through all record having same report name.-->
              <tbody border ="1" class="scrollContent">
                <xsl:choose>
                  <xsl:when test ="($searchType = 'directionalsurveydetail') or ($searchType = 'timedepthdetail')">
                    <xsl:choose>
                      <xsl:when test="/response/report/record[1]/attribute[@name=$sortBy]/@type = 'number'">
                        <xsl:apply-templates select="/response/report/record" mode="TabularDetail">
                          <xsl:sort select="attribute[@name=$sortBy]/@value" order="{$sortType}" data-type="number"/>
                        </xsl:apply-templates>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:apply-templates select="/response/report/record" mode="TabularDetail">
                          <xsl:sort select="attribute[@name=$sortBy]/@value" order="{$sortType}" data-type="text"/>
                        </xsl:apply-templates>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:apply-templates select="/response/report/record" mode="Tabular">
                    </xsl:apply-templates>
                  </xsl:otherwise>
                </xsl:choose>
              </tbody>
            </table>
            <!--End of table-->
            <Script language="javascript">
              var tblShowHideColOption = document.getElementById('tblShowHideColOption');
              var tableDnD = new TableDnD();
              tableDnD.init(tblShowHideColOption);
              <!--some onload scripts are kept in webpart which needs to be run on asynchronous postback,pls check in webpart code for the same-->
            </Script>
          </div>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <span class="labelMessage">
          <xsl:value-of select="'There are no items to show in this view.'"></xsl:value-of>
        </span>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--Root template -->
  <xsl:template match="/response/report/record" mode="Tabular">
    <xsl:if test="(position() &gt;= $recordStarIndex) and (position() &lt;= $recordEndIndex)">
      <tr height="20px">
        <xsl:call-template name="SetAlternateRowColor" />
        <!--column for charting-->
        <xsl:if test="$searchType = 'picksdetail'">
          <xsl:call-template name="RenderLinkToChartColumn">
            <xsl:with-param name="recordNo" select="@recordno"></xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <!--end of column for charting -->
        <xsl:call-template name="RenderRowCheckBoxTd"/>
        <xsl:call-template name="string-tokenizer">
          <xsl:with-param name="templateName" select="'RenderTdCell'" />
          <xsl:with-param name="currentNode" select="." />
          <xsl:with-param name="arrstring" select="$arrColNames" />
          <xsl:with-param name="seperator" select="'|'" />
        </xsl:call-template>
      </tr>
    </xsl:if>
  </xsl:template>
  <xsl:template match="/response/report/record" mode="TabularDetail">
    <xsl:if test="(position() &gt;= $recordStarIndex) and (position() &lt;= $recordEndIndex)">
      <xsl:variable name="recordNumber" select="@recordno"/>
      <tr height="20px">
        <xsl:call-template name="SetAlternateRowColor"/>
        <!--column for charting-->
        <xsl:if test="$searchType = 'directionalsurveydetail'">
          <xsl:call-template name="RenderLinkToChartColumn">
            <xsl:with-param name="recordNo" select="$recordNumber"></xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <!--end of column for charting -->
        <xsl:call-template name="RenderRowCheckBoxTd"/>
        <xsl:call-template name="string-tokenizer">
          <xsl:with-param name="templateName" select="'RenderTdCell'" />
          <xsl:with-param name="currentNode" select="." />
          <xsl:with-param name="arrstring" select="$arrColNames" />
          <xsl:with-param name="seperator" select="'|'" />
        </xsl:call-template>
      </tr>
    </xsl:if>
  </xsl:template>
  <!-- Quality check Template-->
  <xsl:template name="colorCode">
    <xsl:param name="value"/>
    <xsl:element name="td">
      <xsl:attribute name="style">
        background-color:
        <xsl:choose>
          <xsl:when test="$value >= $HighRange">
            <xsl:value-of select='$HighRangeColor'/>
          </xsl:when>
          <xsl:when test="$value >= $MediumRange">
            <xsl:value-of select='$MediumRangeColor'/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select='$LowRangeColor'/>
          </xsl:otherwise>
        </xsl:choose>
        ;text-align:right;
      </xsl:attribute>
      <xsl:value-of select="$value"></xsl:value-of>&#160;
    </xsl:element>
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
      </xsl:when>
      <xsl:otherwise>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="AddDataType">
    <xsl:param name="currentNode"/>
    <xsl:attribute name="type">
      <xsl:value-of select="$currentNode/@type"></xsl:value-of>
    </xsl:attribute>
  </xsl:template>

  <!--Reorder column module templates-->
  <xsl:template name="string-tokenizer">
    <xsl:param name ="templateName" />
    <xsl:param name ="currentNode" />
    <xsl:param name="arrstring" />
    <xsl:param name="seperator" />

    <xsl:variable name="first" select="substring-before($arrstring,$seperator)" />
    <xsl:variable name="remaining" select="substring-after($arrstring,$seperator)" />
    <!--calling template wich will call required method -->
    <xsl:call-template name="CallTemplate">
      <xsl:with-param name="templateName" select="$templateName" />
      <xsl:with-param name="currentNode" select="$currentNode" />
      <xsl:with-param name="first" select="$first" />
    </xsl:call-template>
    <!-- End-->
    <xsl:if test="$remaining">
      <xsl:call-template name="string-tokenizer">
        <xsl:with-param name="templateName" select="$templateName" />
        <xsl:with-param name="currentNode" select="$currentNode" />
        <xsl:with-param name="arrstring" select="$remaining" />
        <xsl:with-param name="seperator" select="$seperator" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <!-- Template which will call other template on the basis of template name-->
  <xsl:template name="CallTemplate">
    <xsl:param name ="templateName" />
    <xsl:param name ="currentNode" />
    <xsl:param name ="first"/>
    <xsl:choose>
      <xsl:when test="$templateName = 'RenderTdCell'">
        <xsl:call-template name="RenderTdCell">
          <xsl:with-param name="currentNode" select="$currentNode" />
          <xsl:with-param name="first" select="$first" />
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$templateName = 'RenderThCell'">
        <xsl:call-template name="RenderThCell">
          <xsl:with-param name="currentNode" select="$currentNode" />
          <xsl:with-param name="first" select="$first" />
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$templateName = 'RenderColTag'">
        <xsl:call-template name="RenderColTag">
          <xsl:with-param name="first" select="$first" />
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$templateName = 'RenderThChkBx'">
        <xsl:call-template name="RenderThChkBx">
          <xsl:with-param name="currentNode" select="$currentNode" />
          <xsl:with-param name="first" select="$first" />
        </xsl:call-template>
      </xsl:when>
    </xsl:choose>

  </xsl:template>
  <!--End-->
  <xsl:template name="RenderTdCell">
    <xsl:param name ="currentNode" />
    <xsl:param name ="first"/>
    <xsl:variable name="currentAttribute" select="$currentNode/attribute[@name=$first]"/>
    <xsl:choose>
      <xsl:when test="($currentAttribute/@name = 'Quality') and ($searchType = 'wellboreheader')">
        <xsl:call-template name="colorCode">
          <xsl:with-param name="value" select="$currentAttribute/@value"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string($currentAttribute/@value)">
            <xsl:element name="td">
              <xsl:variable name="refCol" select="$currentAttribute/@referencecolumn"/>
              <xsl:variable name="refUnit" select="./attribute[@name=$refCol]/@value" />
              <xsl:choose>
                <xsl:when test="(string-length($currentAttribute/@referencecolumn)&gt;0)">
                  <xsl:attribute name="style">
                    <xsl:value-of select="'text-align:right'"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test="$currentAttribute/@value = 'No data'">
                      <xsl:value-of select="$currentAttribute/@value"/>
                    </xsl:when>
                    <xsl:when test="($refUnit = 'metres') or ($refUnit ='feet')">
                      <xsl:call-template name="FeetMeterConvertor">
                        <xsl:with-param name="refUnit" select="$refUnit" />
                        <xsl:with-param name="currentAttribute" select="$currentAttribute" />
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:when test="($refUnit = 'degc') or ($refUnit = 'degf') or ($refUnit = 'degC') or ($refUnit = 'degF')">
                      <xsl:call-template name="TemperatureUnitConvertor">
                        <xsl:with-param name="refUnit" select="$refUnit" />
                        <xsl:with-param name="currentAttribute" select="$currentAttribute" />
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:when test="($refUnit = 'bara') or ($refUnit = 'kpa') or ($refUnit = 'psia')">
                      <xsl:call-template name="PressureUnitConvertor">
                        <xsl:with-param name="refUnit" select="$refUnit" />
                        <xsl:with-param name="currentAttribute" select="$currentAttribute" />
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test ="$currentAttribute/@type='number'">
                  <xsl:attribute name="style">
                    <xsl:value-of select="'text-align:right'"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test="$currentAttribute/@value = 'No data'">
                      <xsl:value-of select="$currentAttribute/@value"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="$currentAttribute/@type = 'date'">
                  <xsl:value-of select="objDate:GetDateTime($currentAttribute/@value)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:call-template name="AddTVDSSCell">
                    <xsl:with-param name="currentNode" select="$currentAttribute"></xsl:with-param>
                  </xsl:call-template>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="td">
              <xsl:call-template name="HideColumn">
                <xsl:with-param name="currentNode" select="$currentAttribute"></xsl:with-param>
              </xsl:call-template>
              &#160;
            </xsl:element>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="RenderThCell">
    <xsl:param name ="currentNode" />
    <xsl:param name ="first"/>
    <xsl:variable name="currentAttribute" select="$currentNode/attribute[@name=$first]"/>
    <xsl:element name="th">
      <xsl:call-template name="AddDataType">
        <xsl:with-param name="currentNode" select="$currentAttribute"></xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="HideColumn">
        <xsl:with-param name="currentNode" select="$currentAttribute"></xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="ApplyToolTip">
        <xsl:with-param name="currentNode" select="$currentAttribute"></xsl:with-param>
      </xsl:call-template>
      <xsl:attribute name="id">
        <xsl:value-of select="$currentAttribute/@name"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="string($currentAttribute/@referencecolumn)">
          <xsl:variable name="refCol" select="$currentAttribute/@referencecolumn"/>
          <xsl:variable name="refUnit" select="./attribute[@name=$refCol]/@value" />
          <xsl:value-of select="$currentAttribute/@name"/>&#160;
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
          <xsl:value-of select="$currentAttribute/@name"/>&#160;
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="RenderSortingArrow">
        <xsl:with-param name="name" select="$currentAttribute/@name" />
      </xsl:call-template>
    </xsl:element>
  </xsl:template>
  <xsl:template name="RenderColTag">
    <xsl:param name ="first"/>
    <col>
      <xsl:attribute name="class">
        <xsl:choose>
          <xsl:when test="$first = 'true'">
            <xsl:text disable-output-escaping="yes">show</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text disable-output-escaping="yes">hide</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </col>
  </xsl:template>
  <xsl:template name="RenderThChkBx">
    <xsl:param name ="currentNode" />
    <xsl:param name ="first"/>
    <xsl:variable name="currentAttribute" select="$currentNode/attribute[@name=$first]"/>
    <th align="center"  text-align="center">
      <xsl:call-template name="HideColumn">
        <xsl:with-param name="currentNode" select="$currentAttribute" />
      </xsl:call-template>
      <xsl:call-template name="RenderChkBxAttribute">
        <xsl:with-param name="name" select="$currentAttribute/@name" />
      </xsl:call-template>
    </th>
  </xsl:template>
  <xsl:template name="RenderSortingArrow">
    <xsl:param name ="name" />
    <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:OnPageLinkClick(</xsl:text>
    <xsl:value-of select="$currentPageNumber"/>,
    '<xsl:value-of select="$name"/>',
    <xsl:choose>
      <xsl:when test="($sortBy = $name) and ($sortType='descending')">
        'ascending'
      </xsl:when>
      <xsl:otherwise>
        'descending'
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
    <img class="hidePrintLink" alt="click to sort" >
      <xsl:attribute name="src">
        <xsl:choose>
          <xsl:when test="$sortBy = $name">
            <xsl:choose>
              <xsl:when test="$sortType='ascending'">
                <xsl:value-of select="'/_layouts/DREAM/images/UP_ACTIVE.gif'" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'/_layouts/DREAM/images/DOWN_ACTIVE.gif'" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'/_layouts/DREAM/images/UP_INACTIVE.gif'" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </img>
    <xsl:text disable-output-escaping="yes">&lt;/a></xsl:text>
  </xsl:template>

  <xsl:template name="RenderReorderLink">
    <xsl:text disable-output-escaping="yes">&lt;a id="linkReorder" style="display:none" href="Javascript:OnPageLinkClick(</xsl:text>
    <xsl:value-of select="$currentPageNumber"/>,
    <xsl:choose>
      <xsl:when test="string($sortBy)">
        '<xsl:value-of select="$sortBy"/>',
      </xsl:when>
      <xsl:otherwise>
        '',
      </xsl:otherwise>
    </xsl:choose>
    '<xsl:value-of select="$sortType"/>')"
    <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
    <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
  </xsl:template>
  <!--End-->
  <!-- charting related templates-->
  <!--Renders chart link column -->
  <xsl:template name="RenderLinkToChartColumn">
    <xsl:param name="recordNo" select="''"></xsl:param>

    <!--td for charting-->
    <td class="checkLocked">
      <xsl:choose>
        <xsl:when test="(position()=1) or (position() = ($recordsPerPage * number($currentPageNumber)+1))">
          <xsl:call-template name="RenderChartLink" >
            <xsl:with-param name="UWIValue" select="./attribute[@name='UWI']/@value"></xsl:with-param>
            <xsl:with-param name="UWBIValue" select="./attribute[@name='UWBI']/@value"></xsl:with-param>
            <xsl:with-param name="recordNumber" select="$recordNo"></xsl:with-param>
            <xsl:with-param name="criteriaName" select="'UWI'"></xsl:with-param>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="position()&gt;1">
          <xsl:variable name="PreviousUWI" select="preceding-sibling::*[1]/attribute[@name='UWI']/@value"/>
          <xsl:variable name="CurrentUWI" select="./attribute[@name='UWI']/@value"/>
          <xsl:variable name="PreviousUWBI" select="preceding-sibling::*[1]/attribute[@name='UWBI']/@value"/>
          <xsl:variable name="CurrentUWBI" select="./attribute[@name='UWBI']/@value"/>
          <xsl:choose >
            <xsl:when test="$PreviousUWBI != $CurrentUWBI">
              <xsl:call-template name="RenderChartLink" >
                <xsl:with-param name="UWIValue" select="./attribute[@name='UWI']/@value"></xsl:with-param>
                <xsl:with-param name="UWBIValue" select="./attribute[@name='UWBI']/@value"></xsl:with-param>
                <xsl:with-param name="recordNumber" select="$recordNo"></xsl:with-param>
                <xsl:with-param name="criteriaName" select="'UWI'"></xsl:with-param>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          &#160;
        </xsl:otherwise>
      </xsl:choose>
    </td>
    <!--end of td for charting -->
  </xsl:template>

  <!--Renders chart image -->
  <xsl:template name="RenderChartLink">
    <xsl:param name="UWBIValue" ></xsl:param>
    <xsl:param name="UWIValue" ></xsl:param>
    <xsl:param name="recordNumber" ></xsl:param>
    <xsl:param name="criteriaName" ></xsl:param>
    <img src="/_layouts/dream/images/chartlink.gif" alt="Chart View" >
      <xsl:attribute name="id">
        <xsl:text disable-output-escaping="yes">imgSelectChartID</xsl:text>
        <xsl:value-of select="$recordNumber"/>
      </xsl:attribute>
      <xsl:attribute name="onclick">
        <xsl:text disable-output-escaping="yes">OpenChartLink(this,'</xsl:text>
        <xsl:value-of select="$UWBIValue"/>
        <xsl:text disable-output-escaping="yes">','</xsl:text>
        <xsl:value-of select="$UWIValue"/>
        <xsl:text disable-output-escaping="yes">','</xsl:text>
        <xsl:value-of select="/response/report/@name"/>
        <xsl:text disable-output-escaping="yes">','</xsl:text>
        <xsl:value-of select="$criteriaName"/>
        <xsl:text disable-output-escaping="yes">');</xsl:text>
      </xsl:attribute>
    </img>
  </xsl:template>
  <!-- End of charting related templates-->
  <!-- Template to set alternate color of row-->
  <xsl:template name="SetAlternateRowColor">
    <xsl:attribute name="style">
      <xsl:choose>
        <xsl:when test="(position() mod 2) = 0">
          <xsl:value-of disable-output-escaping="yes" select="'background:#EFEFEF'"></xsl:value-of>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of disable-output-escaping="yes" select="'background:#FFFFFF'"></xsl:value-of>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
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
  <xsl:template name="FeetMeterConvertor">
    <xsl:param name="refUnit" ></xsl:param>
    <xsl:param name="currentAttribute" />
    <xsl:choose>
      <xsl:when test="$refUnit =$userPreference">
        <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="$userPreference = 'metres'">
        <xsl:value-of select="format-number(($currentAttribute/@value div $formulaValue), '#0.00')"/>
      </xsl:when>
      <xsl:when test="$userPreference = 'feet'">
        <xsl:value-of select="format-number(($currentAttribute/@value * $formulaValue), '#0.00')"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="TemperatureUnitConvertor">
    <xsl:param name="refUnit" />
    <xsl:param name="currentAttribute" />
    <xsl:choose>
      <xsl:when test="$refUnit = $temperatureUnit">
        <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degc') or ($temperatureUnit = 'degC')">
        <!-- c=(f-32)/1.8-->
        <xsl:value-of select="format-number(((($currentAttribute/@value) - 32) div 1.8), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($temperatureUnit = 'degf') or ($temperatureUnit = 'degF')">
        <!-- f=(c*1.8)+32-->
        <xsl:value-of select="format-number(((($currentAttribute/@value) * 1.8)+32), '#0.00')"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="PressureUnitConvertor">
    <xsl:param name="refUnit" />
    <xsl:param name="currentAttribute" />
    <xsl:choose>
      <xsl:when test="$refUnit = $pressureUnit">
        <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'bara') and ($pressureUnit = 'kpa')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 100), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'bara') and ($pressureUnit = 'psia')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 14.503774), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'kpa') and ($pressureUnit = 'bara')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 0.01), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'kpa') and ($pressureUnit = 'psia')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 0.145038), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'psia') and ($pressureUnit = 'bara')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 0.068948), '#0.00')"/>
      </xsl:when>
      <xsl:when test="($refUnit = 'psia') and ($pressureUnit = 'kpa')">
        <xsl:value-of select="format-number(($currentAttribute/@value * 6.894745), '#0.00')"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- Template to create page link start-->
  <xsl:template name ="CreatePagingLink">
    <table>
      <tr>
        <td align="top">
          <xsl:if test="$currentPageNumber != 1">
            <!-- <xsl:call-template name="CreatePageHyperlink">
        <xsl:with-param name="linkNumber" select="1"/>
     <xsl:with-param name="linkText" select="'First'"/>
      </xsl:call-template>-->
            <xsl:call-template name="CreatePageHyperlink">
              <xsl:with-param name="linkNumber" select="$currentPageNumber -1"/>
              <xsl:with-param name="linkText" select="'Previous'"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:call-template name="CreateLink" >
            <xsl:with-param name="startPageNumber" select="$startPageNumber"/>
          </xsl:call-template>
          <xsl:if test="$currentPageNumber != $pageCount">
            <xsl:call-template name="CreatePageHyperlink">
              <xsl:with-param name="linkNumber" select="$currentPageNumber + 1"/>
              <xsl:with-param name="linkText" select="'Next'"/>
            </xsl:call-template>
            <!-- <xsl:call-template name="CreatePageHyperlink">
        <xsl:with-param name="linkNumber" select="$pageCount"/>
        <xsl:with-param name="linkText" select="'Last'"/>
      </xsl:call-template>-->
          </xsl:if>
        </td>
        <td width="10%" align="top">&#160;</td>
        <td align="top">
          <xsl:call-template name="RenderRecordsInfo"/>
        </td>
      </tr>
    </table>
    <br/>
  </xsl:template>
  <xsl:template name ="CreateLink">
    <xsl:param name="startPageNumber"/>
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
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name ="CreatePageHyperlink">
    <xsl:param name="linkNumber"/>
    <xsl:param name="linkText"/>
    <xsl:text disable-output-escaping="yes">&#160;&lt;a href="Javascript:OnPageLinkClick(</xsl:text>
    <xsl:value-of select="$linkNumber"/>,
    <xsl:choose>
      <xsl:when test="string($sortBy)">
        '<xsl:value-of select="$sortBy"/>',
      </xsl:when>
      <xsl:otherwise>
        '',
      </xsl:otherwise>
    </xsl:choose>
    '<xsl:value-of select="$sortType"/>')"
    <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
    <xsl:value-of select="$linkText"/>
    <xsl:text disable-output-escaping="yes">&lt;/a &gt;</xsl:text>
  </xsl:template>
  <xsl:template name ="RenderRecordsInfo">
    <!--<table >
      <tr>-->
    <!--<td>
          <b>
            <xsl:value-of select="$recordCount" />
          </b>
          <xsl:choose>
            <xsl:when test="$recordCount &gt; 1">
              results found; results
            </xsl:when>
            <xsl:otherwise>
              result found; result
            </xsl:otherwise>
          </xsl:choose>
          <b>
            <xsl:value-of select="$recordStarIndex" />
          </b> -
          <xsl:if test="$recordEndIndex &lt; $recordCount">
            <b>
              <xsl:value-of select="$recordEndIndex" />
            </b>
          </xsl:if>
          <xsl:if test="$recordEndIndex &gt;= $recordCount">
            <b>
              <xsl:value-of select="$recordCount" />
            </b>
          </xsl:if>
          shown. &#160; &#160; &#160; &#160;
        </td>-->
    <!--<td >-->
    <xsl:value-of disable-output-escaping="yes" select="$reorderDivHTML"/>
    <!--</td>
      </tr>
    </table>-->
  </xsl:template>
  <!-- Template to create page link end-->
  <xsl:template name="RenderChkBxAttribute">
    <xsl:param name ="name" />
    <xsl:element name="input">
      <xsl:attribute name="type">checkbox</xsl:attribute>
      <xsl:attribute name="id">chkbxColumn</xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="$name"/>
        <xsl:value-of select="'|'"/>
      </xsl:attribute>
      <xsl:attribute name="onclick">
        CheckBoxOnChange(this,'tblSearchResults','thead','chkbxColumnSelectAll','hidColSelectedCheckBoxes');
      </xsl:attribute>
      <xsl:call-template name="String-Equals">
        <xsl:with-param name ="arrString" select="$arrColSelectedCheckboxes"/>
        <xsl:with-param name="stringToCompare" select="$name"/>
        <xsl:with-param name="seperator" select="'|'"/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>
  <xsl:template name ="RenderRowCheckBoxTd">
    <td class="checkLocked">
      <xsl:variable name="value" select="attribute[@contextkey='true'][1]/@value"/>
      <xsl:element name="input">
        <xsl:attribute name="type">checkbox</xsl:attribute>
        <xsl:attribute name="id">chkbxRow</xsl:attribute>
        <xsl:attribute name="value">
          <xsl:value-of select="$value"/>
          <xsl:value-of select="'|'"/>
        </xsl:attribute>
        <xsl:attribute name="assetName">
          <xsl:value-of select="attribute[@name=$assetColName][1]/@value"/>
          <xsl:value-of select="'|'"/>
        </xsl:attribute>
        <xsl:attribute name="onclick">
          CheckBoxOnChange(this,'tblSearchResults','tbody','chkbxRowSelectAll','hidRowSelectedCheckBoxes');
        </xsl:attribute>
        <xsl:if test="contains($arrRowSelectedCheckboxes,$value)">
          <xsl:attribute name="checked">checked</xsl:attribute>
        </xsl:if>
      </xsl:element>
    </td>
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
