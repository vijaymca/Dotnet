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
  <!--MaxRecords-->
  <xsl:param name="MaxRecords"/>
  
  <xsl:param name="searchType" select="EPCatalog"/>
  <xsl:template match="/">      
      <xsl:variable name="flag" select="0"/>    
      <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />
      <!--To do paging with page numbers-->
      <!-- select first element of each page -->
    <xsl:for-each select="ResultSet/Entry[((position()-1) mod $recordsPerPage = 0)]">
        <!-- display the appropriate portion of page links; disable link to active page -->
        <xsl:choose>
          <xsl:when test="(position() > ($CurrentPage - ceiling($MaxPages div 2)) or position() > (last() - $MaxPages)) and ((position() &lt; $CurrentPage + $MaxPages div 2) or (position() &lt; 1 + $MaxPages))">
            
            <xsl:if test="position()=$CurrentPage">
              &#160; <b>
                [ <xsl:value-of select="position()"/> ]
              </b> &#160;
            </xsl:if>
            <xsl:if test="position()!=$CurrentPage">
              <xsl:text disable-output-escaping="yes">&lt;a href="</xsl:text>
              <xsl:value-of select="$CurrentPageName"/>
              <xsl:text disable-output-escaping="yes">&amp;pagenumber=</xsl:text>
              <xsl:value-of select="position() - 1"/>
              <xsl:text disable-output-escaping="yes">&amp;sortby=</xsl:text>
              <xsl:value-of select="$sortBy"/>
              <xsl:text disable-output-escaping="yes">&amp;SearchType=</xsl:text>
              <xsl:value-of select="$searchType"/>
              <xsl:text disable-output-escaping="yes">&amp;assetType=</xsl:text>
              <xsl:value-of select="$assetType"/>
              <xsl:text disable-output-escaping="yes">&amp;sorttype=</xsl:text>
              <xsl:value-of select="$sortType"/>
              <xsl:text disable-output-escaping="yes">&amp;MaxRecords=</xsl:text>
              <xsl:value-of select="$MaxRecords"/>
              <xsl:text disable-output-escaping="yes">"&gt;</xsl:text>
              <xsl:value-of select="position()"/>
              <xsl:text disable-output-escaping="yes">&lt;/a&gt; </xsl:text>
            </xsl:if>
          </xsl:when>

          <xsl:when test="position()=1">
            <xsl:text disable-output-escaping="yes">&lt;a href="</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>
            <xsl:text disable-output-escaping="yes">&amp;pagenumber=</xsl:text>
            <xsl:value-of select="position() - 1"/>
            <xsl:text disable-output-escaping="yes">&amp;sortby=</xsl:text>
            <xsl:value-of select="$sortBy"/>
            <xsl:text disable-output-escaping="yes">&amp;SearchType=</xsl:text>
            <xsl:value-of select="$searchType"/>
            <xsl:text disable-output-escaping="yes">&amp;assetType=</xsl:text>
            <xsl:value-of select="$assetType"/>
            <xsl:text disable-output-escaping="yes">&amp;sorttype=</xsl:text>
            <xsl:value-of select="$sortType"/>
            <xsl:text disable-output-escaping="yes">&amp;MaxRecords=</xsl:text>
            <xsl:value-of select="$MaxRecords"/>
            <xsl:text disable-output-escaping="yes">"&gt;</xsl:text>
            First<xsl:text disable-output-escaping="yes">&lt;/a&gt;&#160;</xsl:text>
          </xsl:when>

          <xsl:when test="position()=last()">
            <xsl:text disable-output-escaping="yes">&lt;a href="</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>
            <xsl:text disable-output-escaping="yes">&amp;pagenumber=</xsl:text>
            <xsl:value-of select="position() - 1"/>
            <xsl:text disable-output-escaping="yes">&amp;sortby=</xsl:text>
            <xsl:value-of select="$sortBy"/>
            <xsl:text disable-output-escaping="yes">&amp;SearchType=</xsl:text>
            <xsl:value-of select="$searchType"/>
            <xsl:text disable-output-escaping="yes">&amp;assetType=</xsl:text>
            <xsl:value-of select="$assetType"/>
            <xsl:text disable-output-escaping="yes">&amp;sorttype=</xsl:text>
            <xsl:value-of select="$sortType"/>
            <xsl:text disable-output-escaping="yes">&amp;MaxRecords=</xsl:text>
            <xsl:value-of select="$MaxRecords"/>
            <xsl:text disable-output-escaping="yes">"&gt;</xsl:text>
            Last
            <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
          </xsl:when>

          <xsl:when test="(position() > ($CurrentPage - ceiling($MaxPages div 2) - 1) or position() > (last() - $MaxPages) - 1 ) and ((position() &lt; $CurrentPage + $MaxPages div 2 + 1) or (position() &lt; 2 + $MaxPages))">
            <xsl:text disable-output-escaping="yes">&lt;a href="</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>
            <xsl:text disable-output-escaping="yes">&amp;pagenumber=</xsl:text>
            <xsl:value-of select="position() - 1"/>
            <xsl:text disable-output-escaping="yes">&amp;sortby=</xsl:text>
            <xsl:value-of select="$sortBy"/>
            <xsl:text disable-output-escaping="yes">&amp;SearchType=</xsl:text>
            <xsl:value-of select="$searchType"/>
            <xsl:text disable-output-escaping="yes">&amp;assetType=</xsl:text>
            <xsl:value-of select="$assetType"/>
            <xsl:text disable-output-escaping="yes">&amp;sorttype=</xsl:text>
            <xsl:value-of select="$sortType"/>
            <xsl:text disable-output-escaping="yes">&amp;MaxRecords=</xsl:text>
            <xsl:value-of select="$MaxRecords"/>
            <xsl:text disable-output-escaping="yes">"&gt;</xsl:text>...<xsl:text disable-output-escaping="yes">&lt;/a&gt; </xsl:text>
          </xsl:when>
        </xsl:choose>
      </xsl:for-each>
      <br></br>
      <!--Looping through each report-->
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
      <table style="border-right:1px solid #336699;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
        <thead class="fixedHeader" id="fixedHeader">
          <tr style="height: 20px;" align="center">
            <th max-width="20px" valign="center"  align="center"  text-align="center">              
              <a href="Javascript:EPCatalogPopup('Status');"><img src="/_layouts/DREAM/images/Attachment/Info.gif"/></a>&#160;Published Status&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Status'"/>
            </xsl:call-template>
          </th>
            <th width="15px" align="center"  text-align="center">Published Date&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Publish Date'"/>
            </xsl:call-template>
          </th>            
            <th width="15px" align="center"  text-align="center">
              <a href="Javascript:EPCatalogPopup('Attachment');">
                <img src="/_layouts/DREAM/images/Attachment/Info.gif"/>
              </a>&#160;Attachment&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Attachment'"/>
            </xsl:call-template>
          </th>            
            <th width="15px" align="center"  text-align="center">Title&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Title'"/>
            </xsl:call-template>
          </th>            
            <th width="15px" align="center"  text-align="center">Asset Name&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Asset Name'"/>
            </xsl:call-template>
          </th>            
            <th width="15px" align="center"  text-align="center">Author&#160;<xsl:call-template name="Sorting">
              <xsl:with-param name="value" select="'Author'"/>
            </xsl:call-template>
          </th>
            <th width="15px" align="center"  text-align="center">Download&#160;</th>
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
      <Script language="javascript">
        SetAlternateColor("tblSearchResults");
        setWindowTitle('Document Search Result');
      </Script>
    </div>
  </xsl:template>
  <xsl:template name="TableBody">
    <xsl:if test="position() &gt; $recordsPerPage * number($pageNumber) and position()
                    &lt;= number($recordsPerPage * number($pageNumber) + $recordsPerPage)">
      <tr height="20px">
        <td max-width="20px"  text-align="center" align="center">
          <xsl:choose>
            <xsl:when test="string(PublishedStatus)">
              <xsl:text disable-output-escaping="yes">&lt;img src="/_layouts/DREAM/images/Status/</xsl:text>
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
        <td text-align="center" align="center">
          <xsl:choose>
            <xsl:when test="string(Usage/Location)">
              <xsl:text disable-output-escaping="yes">&lt;a href="javascript:openAttachment('</xsl:text>
              <xsl:value-of select="Usage/Location"/>')"
              <xsl:text disable-output-escaping="yes">&gt;&lt;img src="/_layouts/DREAM/images/Attachment/</xsl:text><xsl:value-of select="Usage/LogicalFormat"/><xsl:text disable-output-escaping="yes">.gif"&gt;</xsl:text>              
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
          <xsl:attribute name="style">
            text-align:center; cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:downLoadType3Report('</xsl:text>
          <xsl:value-of select="SecurityStatus"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Download</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
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
              <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:EPSorting('</xsl:text>
              <xsl:value-of select="$CurrentPageName"/>',
              '<xsl:value-of select="$pageNumber"/>',
              '<xsl:value-of select="$searchType"/>',
              '<xsl:value-of select="$assetType"/>',
              '<xsl:value-of select="$value"/>','descending')"
              <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
              <img src="/_layouts/DREAM/images/UP_ACTIVE.gif"/>
              <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:EPSorting('</xsl:text>
              <xsl:value-of select="$CurrentPageName"/>',
              '<xsl:value-of select="$pageNumber"/>',
              '<xsl:value-of select="$searchType"/>',
              '<xsl:value-of select="$assetType"/>',
              '<xsl:value-of select="$value"/>','ascending')"
              <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
              <img src="/_layouts/DREAM/images/DOWN_ACTIVE.gif"/>
              <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:EPSorting('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          '<xsl:value-of select="$pageNumber"/>',
          '<xsl:value-of select="$searchType"/>',
          '<xsl:value-of select="$assetType"/>',
          '<xsl:value-of select="$value"/>','ascending')"
          <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
          <img src="/_layouts/DREAM/images/UP_INACTIVE.gif"/>
          <xsl:text disable-output-escaping="yes">&lt;/a&gt;</xsl:text>
        </xsl:otherwise>
      </xsl:choose>    
  </xsl:template>
</xsl:stylesheet>
