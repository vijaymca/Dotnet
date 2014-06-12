<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:objDate="urn:DATE">
  <xsl:param name="recordsPerPage" select="100" />
  <xsl:param name="pageNumber" select="0" />
  <xsl:param name="recordCount" select="0"/>
  <xsl:param name="startValue" select="0"/>
  <xsl:param name="endValue" select="0"/>
  <xsl:param name="MaxPages" select="5"/>
  <xsl:param name="CurrentPage"/>
  <xsl:param name="CurrentPageName" select="''"/>
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>
  <xsl:param name="assetType" select="Well"/>
  <xsl:param name="SortXPath" select=''/>
  <xsl:param name="ColumnsToLock" select="0"/>
  <!--requestId-->
  <xsl:param name="requestId"/>
  <!--MaxRecords-->
  <xsl:param name="MaxRecords"/>

  <xsl:param name="searchType" select="EPCatalog"/>
  <xsl:template match="/">
    <xsl:variable name="flag" select="0"/>
    <!--To do paging with page numbers-->
    <!-- select first element of each page -->
    <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />
    <!--To do paging with page numbers-->
    <xsl:variable name="StartPagenumber" select="0"/>
    <!-- Call Templates of for page number Loop-->
    <xsl:call-template name="Loop">
      <xsl:with-param name="EndPageNumber" select="$Pages"/>
      <xsl:with-param name="StartPagenumber" select="1"/>
    </xsl:call-template>
    <br></br>
    <!--Looping through each report-->
    <xsl:choose>
      <xsl:when test="$recordCount &gt; 0">
        <xsl:for-each select="ResultSet">
          <!--To check the report name-->
          <!--Creating the column headers for report name WELL.-->
          <xsl:variable name="startValue" select="($recordsPerPage * ($pageNumber)) + 1" />
          <xsl:variable name="endValueTemp" select="$recordsPerPage * ($pageNumber + 1)" />
          <b>
            <xsl:value-of select="$recordCount" />
          </b> results found; results

          <b>
            <xsl:value-of select="$startValue" />
          </b> -
          <xsl:if test="$endValueTemp &lt; $recordCount">
            <xsl:variable name="endValue" select="$recordsPerPage * ($pageNumber + 1)" />
            <b>
              <xsl:value-of select="$endValue" />
            </b>
          </xsl:if>
          <xsl:if test="$endValueTemp &gt; $recordCount">
            <xsl:variable name="endValue" select="$recordCount" />
            <b>
              <xsl:value-of select="$endValue" />
            </b>
          </xsl:if>
          <xsl:if test="$endValueTemp = $recordCount">
            <xsl:variable name="endValue" select="$recordCount" />
            <b>
              <xsl:value-of select="$endValue" />
            </b>
          </xsl:if> shown. &#160; &#160; &#160; &#160;
          <br></br>
        </xsl:for-each>

        <div id="tableContainer" class="tableContainer">
          <table style="border-right:1px solid #bdbdbd;"  cellpadding="0" cellspacing="0" 
              class="scrollTable" id="tblSearchResults">
            <thead class="fixedHeader" id="fixedHeader">
              <tr align="center" height="20px">
                <th   id="hidePrintCol" align="center"  text-align="center" style="white-space:nowrap">
                  <a class="hidePrintLink" href="Javascript:EPCatalogPopup('Status');">
                    <img alt="Published Status" class="hidePrintLink" src="/_layouts/DREAM/images/Attachment/Info.gif"/>
                  </a>&#160;Published Status&#160;
                  <xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Status'"/>
                  </xsl:call-template>
                </th>
                <th width="auto" align="center"  text-align="center">
                  <nobr>Published Date&#160;</nobr>
                  <xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Publish Date'"/>
                  </xsl:call-template>
                </th>
                <th id="hidePrintCol" align="center"  text-align="center">
                  <a class="hidePrintLink" href="Javascript:EPCatalogPopup('Attachment');">
                    <img alt="Attachment" class="hidePrintLink" src="/_layouts/DREAM/images/Attachment/Info.gif"/>
                  </a>&#160;Attachment&#160;<xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Attachment'"/>
                  </xsl:call-template>
                </th>
                <th align="center"  text-align="center">
                  Title&#160;<xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Title'"/>
                  </xsl:call-template>
                </th>
                <th align="center"  text-align="center">
                  <nobr>Asset Name&#160;</nobr>
                  <xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Asset Name'"/>
                  </xsl:call-template>
                </th>
                <th align="center"  text-align="center">
                  Author&#160;<xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Author'"/>
                  </xsl:call-template>
                </th>
                <th align="center"  text-align="center">
                  Product Type&#160;<xsl:call-template name="Sorting">
                    <xsl:with-param name="value" select="'Product Type'"/>
                  </xsl:call-template>
                </th>
              </tr>
            </thead>
            <tbody boder ="1" class="scrollContent">
              <xsl:choose>
                <xsl:when test="$SortXPath = 'Usage/LogicalFormat'">
                  <xsl:for-each select="ResultSet/Entry/Attributes">
                    <!--sorting columns from colname and order as param-->
                    <xsl:sort select="Usage/LogicalFormat" order="{$sortType}"/>
                    <xsl:call-template name="TableBody"></xsl:call-template>
                  </xsl:for-each>
                </xsl:when>
                <xsl:when test="$sortBy = 'Product Type'">
                  <xsl:for-each select="ResultSet/Entry/Attributes">
                    <!--sorting columns from colname and order as param-->
                    <xsl:sort select="Context/ProductType" order="{$sortType}"/>
                    <xsl:call-template name="TableBody"></xsl:call-template>
                  </xsl:for-each>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$SortXPath = 'Author/Name'">
                      <xsl:for-each select="ResultSet/Entry/Attributes">
                        <!--sorting columns from colname and order as param-->
                        <xsl:sort select="Author/Name" order="{$sortType}"/>
                        <xsl:call-template name="TableBody"></xsl:call-template>
                      </xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:for-each select="ResultSet/Entry/Attributes">
                        <!--sorting columns from colname and order as param-->
                        <xsl:sort select="*[name() = $SortXPath]" order="{$sortType}"/>
                        <xsl:call-template name="TableBody"></xsl:call-template>
                      </xsl:for-each>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </tbody>
          </table>
        </div>
      </xsl:when>
      <xsl:otherwise>
        &#160;<span style="color:Red">No Documents found. </span>
      </xsl:otherwise>
    </xsl:choose>
    <Script language="javascript">
      SetAlternateColor("tblSearchResults");
      setWindowTitle('EPCatalogue Search Result');
    </Script>

  </xsl:template>
  <xsl:template name="TableBody">
    <xsl:if test="position() &gt; $recordsPerPage * number($pageNumber) and position()
                    &lt;= number($recordsPerPage * number($pageNumber) + $recordsPerPage)">
      <tr height="20px">
        <td id="hidePrintCol" max-width="20px"  text-align="center" align="center">
          <xsl:choose>
            <xsl:when test="string(PublishedStatus)">
              <xsl:text disable-output-escaping="yes">&lt;img class="hidePrintLink" src="/_layouts/DREAM/images/Status/</xsl:text>
              <xsl:value-of select="PublishedStatus"/>
              <xsl:text disable-output-escaping="yes">.gif"/&gt;</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="string(PublishDate)">
              <xsl:value-of select="objDate:GetDateTime(PublishDate)"/>
              <!--<xsl:value-of select="PublishDate"/>-->
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td id="hidePrintCol" text-align="center" align="center">
          <xsl:choose>
            <xsl:when test="string(Usage/Location)">
              <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="javascript:openAttachment('</xsl:text>
              <xsl:value-of select="Usage/Location"/>')"
              <xsl:text disable-output-escaping="yes">&gt;&lt;img class="hidePrintLink" src="/_layouts/DREAM/images/Attachment/</xsl:text><xsl:value-of select="Usage/LogicalFormat"/><xsl:text disable-output-escaping="yes">.gif"&gt;</xsl:text>
              <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="string(Title)">
              <xsl:value-of select="Title"/>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="string(Asset)">
              <xsl:value-of select="Asset"/>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="string(Author/Name)">
              <xsl:value-of select="Author/Name"/>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="string(Context/ProductType)">
              <xsl:value-of select="Context/ProductType"/>
            </xsl:when>
            <xsl:otherwise>
              &#160;
            </xsl:otherwise>
          </xsl:choose>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
  <xsl:template name="Sorting">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="$sortBy = $value">
        <xsl:choose>
          <xsl:when test="$sortType='ascending'">
            <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:EPSorting('</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>',
            <xsl:value-of select="$pageNumber"/>,
            <xsl:value-of select="$recordCount"/>,
            '<xsl:value-of select="$requestId"/>',
            '<xsl:value-of select="$value"/>',
            'descending',
            <xsl:value-of select="$MaxRecords"/>,
            '<xsl:value-of select="$searchType"/>',
            '<xsl:value-of select="$assetType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
            <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_ACTIVE.gif"/>
            <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:EPSorting('</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>',
            <xsl:value-of select="$pageNumber"/>,
            <xsl:value-of select="$recordCount"/>,
            '<xsl:value-of select="$requestId"/>',
            '<xsl:value-of select="$value"/>',
            'ascending',
            <xsl:value-of select="$MaxRecords"/>,
            '<xsl:value-of select="$searchType"/>',
            '<xsl:value-of select="$assetType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
            <img class="hidePrintLink" src="/_layouts/DREAM/images/DOWN_ACTIVE.gif"/>
            <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:EPSorting('</xsl:text>
        <xsl:value-of select="$CurrentPageName"/>',
        <xsl:value-of select="$pageNumber"/>,
        <xsl:value-of select="$recordCount"/>,
        '<xsl:value-of select="$requestId"/>',
        '<xsl:value-of select="$value"/>',
        'ascending',
        <xsl:value-of select="$MaxRecords"/>,
        '<xsl:value-of select="$searchType"/>',
        '<xsl:value-of select="$assetType"/>')"
        <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
        <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_INACTIVE.gif"/>
        <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
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
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:EPPaging('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber - 1"/>,
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
          '<xsl:value-of select="$sortType"/>',
          <xsl:value-of select="$MaxRecords"/>,
          '<xsl:value-of select="$searchType"/>',
          '<xsl:value-of select="$assetType"/>')"
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
</xsl:stylesheet>
