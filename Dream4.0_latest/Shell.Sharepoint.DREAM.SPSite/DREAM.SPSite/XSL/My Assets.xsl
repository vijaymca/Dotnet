<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:objDate="urn:DATE">
  <!--Set the paging characteristics - number of records per page, page number and the record count-->
  <!-- Set the number of records per page-->
  <xsl:output method="xml"/>
  <!--Set the paging characteristics - number of records per page, page number and the record count-->
  <!-- Set the number of records per page-->
  <xsl:param name="recordsPerPage" select="0" />
  <xsl:param name="windowTitle" select="''"/>
  <xsl:param name="ColumnsToLock" select="0"/>
  <!--Display Unit Value-->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:variable name="unitValue" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="arrColNames" select="''"/>
  <xsl:param name="arrColDisplayStatus" select="''" />
  <!-- Data Quality Check-->
  <xsl:param name="LowRange"/>
  <xsl:param name="MediumRange"/>
  <xsl:param name="HighRange"/>
  <xsl:param name="LowRangeColor"/>
  <xsl:param name="MediumRangeColor"/>
  <xsl:param name="HighRangeColor"/>

  <xsl:param name="CurrentPageName" select="''"/>
  <!--MaxRecords-->
  <xsl:param name="MaxRecords"/>
  <!--Record Count Field-->
  <xsl:param name="recordCount" select="0"/>
  <!--requestId-->
  <xsl:param name="requestId"/>
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>

  <xsl:param name="searchType" select="''"></xsl:param>
  <!--CurrentPage-->
  <xsl:param name="CurrentPage"/>

  <!--max pages-->
  <xsl:param name="MaxPages" select="5"/>
  <!--Selecting checkboxes-->
  <xsl:param name ="arrRowSelectedCheckboxes" select="''"/>
  <xsl:param name ="arrColSelectedCheckboxes" select="''"/>
  <xsl:param name ="assetColName" select="''"/>

  <!-- End Data Quality Check-->
  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />

    <xsl:if test="$Pages &gt; 1">

      <!--To do paging with page numbers-->
      <xsl:variable name="StartPagenumber" select="0"/>
      <!-- Call Templates of for page number Loop-->
      <xsl:call-template name="Loop">
        <xsl:with-param name="EndPageNumber" select="$Pages"/>
        <xsl:with-param name="StartPagenumber" select="1"/>
      </xsl:call-template>
    </xsl:if>
    <!--Variable declared to terminate the column header generator loop-->
    <xsl:variable name="flag" select="0"/>
    <xsl:for-each select="response/report">
      <!--Creating Scrollbar.-->
      <div id="tableContainer" class="tableContainer">
        <!--table to display response data.-->
        <table style="border-right:1px solid #BDBDBD;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
          <!-- Adding search name as an attribute to get name of search type for current page-->
          <xsl:attribute name="searchType">
            <xsl:value-of select="$searchType"/>
          </xsl:attribute>
          <!--Name of assetname col ex for Well asset 'Well Name' -->
          <xsl:attribute name="assetColName">
            <xsl:value-of select="$assetColName"/>
          </xsl:attribute>
          <!-- Adding col tag for showing hiding columns-->
          <col class="show printHide"></col>
          <xsl:call-template name="string-tokenizer">
            <xsl:with-param name="templateName" select="'RenderColTag'" />
            <xsl:with-param name="arrstring" select="$arrColDisplayStatus" />
            <xsl:with-param name="seperator" select="'|'" />
          </xsl:call-template>
          <!--End of Adding col tag-->
          <xsl:for-each select="record[@recordno=1]">
            <thead class="fixedHeader" id="fixedHeader">

              <tr id="hidePrintRow" style="height: 20px;" align="center">

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
                <th width="15px" text-align="center">
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
          <tbody boder ="1" class="scrollContent">
            <xsl:apply-templates select="/response/report/record" mode="Tabular">
            </xsl:apply-templates>
          </tbody>
        </table>
        <!--End of table-->
        <Script language="javascript">
          lockCol("tblSearchResults", '0');
          SetAlternateColor("tblSearchResults");
          setWindowTitle('<xsl:value-of select="$windowTitle"/>');
          setSelectedMapIdentifier("tblSearchResults");
        </Script>
      </div>
    </xsl:for-each>

  </xsl:template>




  <!--Root template -->
  <xsl:template match="/response/report/record" mode="Tabular">
    <xsl:if test="position() &lt;= $recordsPerPage">
      <xsl:variable name="recordNumber" select="@recordno"/>
      <tr height="20px">
        <td width="15px" text-align="center">
          <xsl:call-template name="RenderRowCheckBoxTd"/>
        </td>
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
      <xsl:value-of select="@value"/>&#160;
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
          <xsl:with-param name="first" select="$first" />
          <xsl:with-param name="currentNode" select="$currentNode" />
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
      <xsl:when test="$currentAttribute/@name = 'Quality'">
        <xsl:call-template name="colorCode">
          <xsl:with-param name="value" select="$currentAttribute/@value"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string($currentAttribute/@value)">
            <xsl:element name="td">
              <xsl:variable name="refCol" select="$currentAttribute/@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose>
                <xsl:when test="(string-length($currentAttribute/@referencecolumn)&gt;0)">
                  <xsl:attribute name="style">
                    <xsl:value-of select="'text-align:right'"/>
                  </xsl:attribute>
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
                </xsl:when>
                <xsl:when test ="$currentAttribute/@type='number'">
                  <xsl:attribute name="style">
                    <xsl:value-of select="'text-align:right'"/>
                  </xsl:attribute>
                  <xsl:value-of select="format-number($currentAttribute/@value, '#0.00')"/>
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
          <xsl:value-of select="$currentAttribute/@name"/>&#160;
          <xsl:choose>
            <xsl:when test="$userPreference = 'metres'">
              (m)&#160;
            </xsl:when>
            <xsl:otherwise>
              (ft)&#160;
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$currentAttribute/@name"/>&#160;
        </xsl:otherwise>
      </xsl:choose>
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
  <!--End-->

  <!--Create Paging Template-->
  <xsl:template name="Loop">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>
    <xsl:if test="$StartPagenumber &lt;= $EndPageNumber">
      <xsl:choose>
        <xsl:when test="$StartPagenumber=$CurrentPage">
          &#160; <b>
            [ <xsl:value-of select="$StartPagenumber"/> ]
          </b> &#160;
        </xsl:when>
        <xsl:otherwise>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:MyAssetPaging('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber - 1"/>,
          <xsl:value-of select="$MaxRecords"/>,
          <xsl:value-of select="$recordCount"/>,
          '<xsl:value-of select="$requestId"/>',
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>
          '<xsl:value-of select="$sortType"/>','<xsl:value-of select="$searchType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
          <xsl:choose>
            <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2)) or $StartPagenumber > ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
              <xsl:value-of select="$StartPagenumber"/>
            </xsl:when>
            <xsl:when test="$StartPagenumber=1">
              <xsl:text disable-output-escaping="yes">First</xsl:text>
            </xsl:when>
            <xsl:when test="$StartPagenumber=$EndPageNumber">
              <xsl:text disable-output-escaping="yes">Last</xsl:text>
            </xsl:when>
            <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2) - 1) or $StartPagenumber > ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
              <xsl:text disable-output-escaping="yes">...</xsl:text>
            </xsl:when>
          </xsl:choose>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="Loop">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <!--Templates for rendering column check boxes-->

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
  <xsl:template name ="RenderRowCheckBoxTd">
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
