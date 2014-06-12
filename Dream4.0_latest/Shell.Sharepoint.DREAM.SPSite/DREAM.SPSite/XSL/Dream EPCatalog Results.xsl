<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:objDate="urn:DATE">

  <xsl:output method="xml"/>
  <!--parameter declaration start here-->
  
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
  <xsl:param name="searchType" select="'EPCatalog'" />

  <!--Pagination parameter declaration ends here-->
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>
  <xsl:param name="SortXPath" select=''/>
  
  <!--parameter declaration ends here-->
  
  <xsl:template match="/">

    <!-- Call Templates of for page number Loop-->
    <xsl:if test="$pageCount &gt; 0">
      <xsl:call-template name="CreatePagingLink"/>
    </xsl:if>
    <br></br>
    <!--Looping through each report-->
    <xsl:choose>
      <xsl:when test="$recordCount &gt; 0">
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
    <xsl:if test="position() &gt; $recordStarIndex
                    &lt;= $recordEndIndex">
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
    <xsl:call-template name="RenderSortingArrow">
      <xsl:with-param name="name" select="$value" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="RenderSortingArrow">
    <xsl:param name ="name" />
    <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:EPOnSortLinkClick(</xsl:text>
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
    <xsl:text disable-output-escaping="yes">&#160;&lt;a href="Javascript:EPOnPageLinkClick(</xsl:text>
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
  <!-- Template to create page link end-->
</xsl:stylesheet>
