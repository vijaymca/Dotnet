<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:objDate="urn:DATE">
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
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="sortBy" select="''"/>
  <xsl:param name="sortType" select="'descending'"/>
  <xsl:param name="activeDiv"/>
  
  <!--End of parameter declaration -->

  <!--Template to create the Response table.-->
  <xsl:template match="/" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">
    <div id="tabs">
      <div class="gray_embossed_tabs_r" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
          <tr>
            <td>
              <ul class="tabNavigation">
                <li>
                  <a href="javascript:tabs('tab-0');">
                    <span id="spanEDM0">Events</span>
                  </a>
                </li>
                <li>
                  <a href="javascript:tabs('tab-1');">
                    <span id="spanEDM1">Daily Summary</span>
                  </a>
                </li>
                <li>
                  <a href="javascript:tabs('tab-2');">
                    <span id="spanEDM2">Reported Activities</span>
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
      <div class="tabbedWindow" id="tab-0">
        <!--for paging     -->
        <!-- Call Templates of for page number Loop-->
        <xsl:variable name="totalRecord" select="count(response/report/record[(attribute/@name='Level') and (attribute/@value='1')])"></xsl:variable>
        <!-- Call Templates of for page number Loop-->
        <xsl:if test="$totalRecord&gt;0">
          <xsl:call-template name="Paging">
            <xsl:with-param name="divName" select="'tab-0'"/>
            <xsl:with-param name="reportName" select="'1'"/>
          </xsl:call-template>
        </xsl:if>
        <!-- End Call Templates of for page number Loop-->

        <!--end paging     -->
        <div id="tableContainer">
          <xsl:choose>
            <xsl:when test="$totalRecord&gt;0">
              <xsl:attribute name="class">
                tableContainer
              </xsl:attribute>
              <table style="border-right:1px solid #bdbdbd;" cellpadding="0" cellspacing="0" id="tblSearchResults"  class="scrollTable" >
                <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='1')]">
                  <xsl:variable name="counter" select="position()"/>
                  <xsl:if test="$counter =1">
                    <thead id="fixedHeader" class="fixedHeader">
                      <tr style="height: 20px;">
                        <xsl:for-each select="attribute">
                          <xsl:if test="@display!='false'">
                            <th style="text-align:center;width:auto;">
                              <xsl:call-template name="AddDataType">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:call-template name="ApplyToolTip">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:value-of select="@name"/>
                            </th>
                          </xsl:if>
                        </xsl:for-each>
                      </tr>
                    </thead>
                  </xsl:if>
                </xsl:for-each>
                <tbody class="scrollContent" >
                  <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='1')]">
                    <xsl:choose>
                      <xsl:when test="$activeDiv='tab-0'">
                        <xsl:if test="(position()&gt;=$recordStarIndex)and(position()&lt;=$recordEndIndex)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:if test="(position()&gt;=1)and(position()&lt;=$recordsPerPage)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:for-each>
                </tbody>
              </table>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>There is no item to show in this view.</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </div>
      </div>
      <div class="tabbedWindow" id="tab-1" style="display:none;">
        <!--for paging     -->
        <!-- Call Templates of for page number Loop-->
        <xsl:variable name="totalRecord" select="count(response/report/record[(attribute/@name='Level') and (attribute/@value='2')])"></xsl:variable>

        <!-- Call Templates of for page number Loop-->
        <xsl:if test="$totalRecord&gt;0">
          <xsl:call-template name="Paging">
            <xsl:with-param name="divName" select="'tab-1'"/>
            <xsl:with-param name="reportName" select="'2'"/>
          </xsl:call-template>
        </xsl:if>
        <!-- End Call Templates of for page number Loop-->

        <!--end paging     -->

        <div id="tableContainer">
          <xsl:choose>
            <xsl:when test="$totalRecord&gt;0">
              <xsl:attribute name="class">
                tableContainer
              </xsl:attribute>
              <table style="border-right:1px solid #bdbdbd;" cellpadding="0" cellspacing="0" id="tblSearchResults"  class="scrollTable" >
                <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='2')]">
                  <xsl:variable name="counter" select="position()"/>
                  <xsl:if test="$counter =1">
                    <thead id="fixedHeader" class="fixedHeader">
                      <tr style="height: 20px;">
                        <xsl:for-each select="attribute">
                          <xsl:if test="@display!='false'">
                            <th style="text-align:center;width:auto;">
                              <xsl:call-template name="AddDataType">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:call-template name="ApplyToolTip">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:value-of select="@name"/>
                            </th>
                          </xsl:if>
                        </xsl:for-each>
                      </tr>
                    </thead>
                  </xsl:if>
                </xsl:for-each>
                <tbody class="scrollContent" >
                  <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='2')]">
                    <xsl:choose>
                      <xsl:when test="$activeDiv='tab-1'">
                        <xsl:if test="(position()&gt;=$recordStarIndex)and(position()&lt;=$recordEndIndex)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:if test="(position()&gt;=1)and(position()&lt;=$recordsPerPage)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:for-each>
                </tbody>
              </table>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>There is no item to show in this view.</xsl:text>
            </xsl:otherwise>
          </xsl:choose>

        </div>
      </div>
      <div class="tabbedWindow" id="tab-2" style="display:none;">
        <!--for paging     -->

        <xsl:variable name="totalRecord" select="count(response/report/record[(attribute/@name='Level') and (attribute/@value='3')])"></xsl:variable>
        <!-- Call Templates of for page number Loop-->
        <xsl:if test="$totalRecord&gt;0">
          <xsl:call-template name="Paging">
            <xsl:with-param name="divName" select="'tab-2'"/>
            <xsl:with-param name="reportName" select="'3'"/>
          </xsl:call-template>
        </xsl:if>
        <!-- End Call Templates of for page number Loop-->

        <!--end paging     -->
        <div id="tableContainer">
          <xsl:choose>
            <xsl:when test="$totalRecord&gt;0">
              <xsl:attribute name="class">
                tableContainer
              </xsl:attribute>
              <table style="border-right:1px solid #bdbdbd;" cellpadding="0" cellspacing="0" id="tblSearchResults"  class="scrollTable" >
                <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='3')]">
                  <xsl:variable name="counter" select="position()"/>
                  <xsl:if test="$counter =1">
                    <thead id="fixedHeader" class="fixedHeader">
                      <tr style="height: 20px;">
                        <xsl:for-each select="attribute">
                          <xsl:if test="@display!='false'">
                            <th style="text-align:center;width:auto;">
                              <xsl:call-template name="AddDataType">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:call-template name="ApplyToolTip">
                                <xsl:with-param name="currentNode" select="."></xsl:with-param>
                              </xsl:call-template>
                              <xsl:value-of select="@name"/>
                            </th>
                          </xsl:if>
                        </xsl:for-each>
                      </tr>
                    </thead>
                  </xsl:if>
                </xsl:for-each>
                <tbody class="scrollContent" >
                  <xsl:for-each select="response/report/record[(attribute/@name='Level') and (attribute/@value='3')]">
                    <xsl:choose>
                      <xsl:when test="$activeDiv='tab-2'">
                        <xsl:if test="(position()&gt;=$recordStarIndex)and(position()&lt;=$recordEndIndex)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:if test="(position()&gt;=1)and(position()&lt;=$recordsPerPage)">
                          <xsl:call-template name="CreateRow">
                            <xsl:with-param name="currentNode" select="."></xsl:with-param>
                          </xsl:call-template>
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:for-each>
                </tbody>
              </table>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>There is no item to show in this view.</xsl:text>
            </xsl:otherwise>
          </xsl:choose>

        </div>
      </div>
      <Script language="javascript">
        tabs('<xsl:value-of select="$activeDiv"/>');
        setWindowTitle('Daily Wells Reporting');
      </Script>
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
  
  <xsl:template name="CreateRow">
    <xsl:param name="currentNode"></xsl:param>
    <tr>
      <xsl:for-each select="$currentNode/attribute">
        <xsl:if test="@display!='false'">
          <xsl:choose>
            <xsl:when test="string-length(string(@value))&gt;0">
              <xsl:choose>
                <xsl:when test="@type = 'number'">
                  <td style="text-align:right;width:auto">
                    <xsl:value-of select="format-number(@value, '#0.00')"/>
                  </td>
                </xsl:when>
                <xsl:when test="@type = 'date'">
                  <td style="width:auto">
                    <xsl:value-of select="@value"/>
                  </td>
                </xsl:when>

                <xsl:otherwise>
                  <td style="width:auto">
                    <xsl:value-of select="@value"/>
                  </td>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <td>&#160;</td>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </xsl:for-each>
    </tr>

  </xsl:template>

  <xsl:template name="AddDataType">
    <xsl:param name="currentNode"/>
    <xsl:attribute name="type">
      <xsl:value-of select="$currentNode/@type"></xsl:value-of>
    </xsl:attribute>
  </xsl:template>
  <!--Create Paging Template-->
  <xsl:template name="Paging">
    <xsl:param name="divName"></xsl:param>
    <xsl:param name="reportName"></xsl:param>
    <xsl:variable name="totalRecord" select="count(response/report/record[(attribute/@name='Level') and (attribute/@value=$reportName)])"></xsl:variable>
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
    '<xsl:value-of select="$sortType"/>',this.parentElement.id)"
    <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
    <xsl:value-of select="$linkText"/>
    <xsl:text disable-output-escaping="yes">&lt;/a &gt;</xsl:text>
  </xsl:template>
  <!-- Template to create page link end-->
</xsl:stylesheet>