<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:objDate="urn:DATE">
   
     
  <!--Set the paging characteristics - number of records per page, page number and the record count-->
  <!-- Set the number of records per page-->    
  <xsl:output method="xml"/>

  <xsl:param name="recordsPerPage" select="0" />
  <!-- Page Number field -->
  <xsl:param name="pageNumber" select="1" />
  <!--Record Count Field-->
  <xsl:param name="recordCount" select="0"/>
  <!--start value-->
  <xsl:param name="startValue" select="0"/>
  <!--end value-->
  <xsl:param name="endValue" select="0"/>
  <xsl:param name="windowTitle" select="''"/>
  <xsl:param name="ColumnsToLock" select="0"/>
  <!--max pages-->
  <xsl:param name="MaxPages" select="5"/>
  <!--CurrentPage-->
  <xsl:param name="CurrentPage"/>
  <!--requestId-->
  <xsl:param name="requestId"/>
  <!--MaxRecords-->
  <xsl:param name="MaxRecords"/>
  <!--Display Unit Value-->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:variable name="unitValue" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>

  <!--CurrentPage-->
  <xsl:param name="CurrentPageName" select="''"/>
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
  <!--Template to create the Response table.-->
  <xsl:template match="/">

    <!--Variable declared to terminate the column header generator loop-->
    <xsl:variable name="flag" select="0"/>

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
    <xsl:for-each select="response/report">
      <xsl:variable name="startValue" select="($recordsPerPage * ($pageNumber)) + 1" />
      <xsl:variable name="endValueTemp" select="$recordsPerPage * ($pageNumber + 1)" />
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
      <!-- End Of Show previous/next page links-->
      <!--Creating Scrollbar.-->
      <div id="tableContainer" class="tableContainer">
        <!--table to display response data.-->
        <!--<xsl:value-of select="myObj:GetDateTime('2006-08-22T06:30:07.7199222-04:00')"/>-->        
        <table style="border-right:1px solid #BDBDBD;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
          <xsl:for-each select="record[@recordno=1]">
            <thead class="fixedHeader" id="fixedHeader">
              <tr id="hidePrintRow" style="height: 20px;" align="center">
                <th width="15px" text-align="center">
                  <xsl:element name="input">
                    <xsl:attribute name="type">checkbox</xsl:attribute>
                    <xsl:attribute name="name">chbColSelectAll</xsl:attribute>
                    <xsl:attribute name="id">chkColSelBox</xsl:attribute>
                    <xsl:attribute name="value">ColIdentifier</xsl:attribute>
                    <xsl:attribute name="Checked">true</xsl:attribute>
                  <xsl:attribute name="onclick">SellectAllColumns('chkColSelBox','cmdColCheckAll')</xsl:attribute>
                  </xsl:element>
                </th>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title = 'true'">
                    <th id="hidePrintCol" width="15px" align="center"  text-align="center">
                     <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                      <xsl:element name="input">
                        <xsl:attribute name="type">checkbox</xsl:attribute>
                        <xsl:attribute name="name">chbColSelectAll</xsl:attribute>
                        <xsl:attribute name="id">chbSelectColID</xsl:attribute>
                        <xsl:attribute name="Checked">true</xsl:attribute>
                        <xsl:attribute name="value">
                          <xsl:value-of select="@name"/>|
                        </xsl:attribute>
                        <xsl:attribute name="onclick">DeSelectCheck('chkColSelBox')</xsl:attribute>                      
                      </xsl:element>
                    </th>
                  </xsl:if>
                </xsl:for-each>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title != 'true'">
                    <th id="hidePrintCol" width="15px" align="center"  text-align="center">
                     <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                      <xsl:element name="input">
                        <xsl:attribute name="type">checkbox</xsl:attribute>
                        <xsl:attribute name="name">chbColSelectAll</xsl:attribute>
                        <xsl:attribute name="id">chbSelectColID</xsl:attribute>
                        <xsl:attribute name="Checked">true</xsl:attribute>
                        <xsl:attribute name="value">
                          <xsl:value-of select="@name"/>|
                        </xsl:attribute>
                        <xsl:attribute name="onclick">DeSelectCheck('chkColSelBox')</xsl:attribute>                      
                      </xsl:element>
                    </th>
                  </xsl:if>
                </xsl:for-each>
              </tr >
              <tr style="height: 20px;">
                <th id="hidePrintCol" width="15px" text-align="center">
                  <xsl:element name="input">
                    <xsl:attribute name="type">checkbox</xsl:attribute>
                    <xsl:attribute name="name">chbSelectAll</xsl:attribute>
                    <xsl:attribute name="id">chkBox</xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:for-each select="attribute">
                        <xsl:if test="@contextkey='true'">
                          <xsl:value-of select="@name"/>
                        </xsl:if>
                      </xsl:for-each>
                    </xsl:attribute>
                    <xsl:attribute name="onclick">SelectAll('chkBox','cmdCheckAll')</xsl:attribute>
                  </xsl:element>
                </th>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title = 'true'">
                    <xsl:element name="th">
                      <xsl:call-template name="AddDataType">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                     <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                    <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                    </xsl:call-template>
                      <xsl:attribute name="id">
                        <xsl:value-of select="@name"/>
                      </xsl:attribute>
                      <xsl:choose>
                        <xsl:when test="string(@referencecolumn)">
                          <xsl:value-of select="@name"/>&#160;(<xsl:choose>
                            <xsl:when test="$userPreference = 'metres'">m)&#160;
                            </xsl:when>
                            <xsl:otherwise>ft)&#160;
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@name"/>&#160;
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:choose>
                        <xsl:when test="$sortBy = @name">
                          <xsl:choose>
                            <xsl:when test="$sortType='ascending'">
                              <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="$requestId"/>',
                              '<xsl:value-of select="@name"/>','descending',
                              <xsl:value-of select="$MaxRecords"/>
                              <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                              <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_ACTIVE.gif" alt="click to sort" />
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="$requestId"/>',
                              '<xsl:value-of select="@name"/>','ascending',
                              <xsl:value-of select="$MaxRecords"/>
                              <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                              <img class="hidePrintLink" src="/_layouts/DREAM/images/DOWN_ACTIVE.gif" alt="click to sort" />
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,
                          <xsl:value-of select="$recordCount"/>,
                          '<xsl:value-of select="$requestId"/>',
                          '<xsl:value-of select="@name"/>','descending',
                          <xsl:value-of select="$MaxRecords"/>
                          <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                          <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_INACTIVE.gif" alt="click to sort" />
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:element>
                  </xsl:if>
                </xsl:for-each>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title != 'true'">
                    <xsl:element name="th">
                     <xsl:call-template name="AddDataType">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                     <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                    <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                    </xsl:call-template>
                      <xsl:attribute name="id">
                        <xsl:value-of select="@name"/>
                      </xsl:attribute>
                      <xsl:choose>
                        <xsl:when test="string(@referencecolumn)">
                          <xsl:value-of select="@name"/>&#160;(<xsl:choose>
                            <xsl:when test="$userPreference = 'metres'">m)&#160;
                            </xsl:when>
                            <xsl:otherwise>ft)&#160;
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@name"/>&#160;
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:choose>
                        <xsl:when test="$sortBy = @name">
                          <xsl:choose>
                            <xsl:when test="$sortType='ascending'">
                              <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="$requestId"/>',
                              '<xsl:value-of select="@name"/>','descending',
                              <xsl:value-of select="$MaxRecords"/>
                              <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                              <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_ACTIVE.gif" alt="click to sort" />
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="$requestId"/>',
                              '<xsl:value-of select="@name"/>','ascending',
                              <xsl:value-of select="$MaxRecords"/>
                              <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                              <img class="hidePrintLink" src="/_layouts/DREAM/images/DOWN_ACTIVE.gif" alt="click to sort" />
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes">&lt;a class="hidePrintLink" href="Javascript:QuickSearchSorting('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,
                          <xsl:value-of select="$recordCount"/>,
                          '<xsl:value-of select="$requestId"/>',
                          '<xsl:value-of select="@name"/>','descending',
                          <xsl:value-of select="$MaxRecords"/>
                          <xsl:text disable-output-escaping="yes">)" style="display:inline"></xsl:text>
                          <img class="hidePrintLink" src="/_layouts/DREAM/images/UP_INACTIVE.gif" alt="click to sort" />
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:element>
                  </xsl:if>
                </xsl:for-each>
              </tr>
            </thead>                              
            </xsl:for-each>          
          <!--Looping through all record having same report name.-->
          <tbody boder ="1" class="scrollContent">
            <xsl:for-each select="/response/report/record">              
              <xsl:if test="position() &lt;= $recordsPerPage">
                <xsl:variable name="recordNumber" select="@recordno"/>
                <tr height="20px">
                  <td id="hidePrintCol" width="15px" text-align="center">
                    <xsl:element name="input">
                      <xsl:attribute name="type">checkbox</xsl:attribute>
                      <xsl:attribute name="name">chbSelectAll1</xsl:attribute>
                      <xsl:attribute name="id">chbSelectID</xsl:attribute>
                      <xsl:attribute name="value">
                        <xsl:for-each select="attribute">
                          <xsl:if test="@contextkey='true'">
                            <xsl:value-of select="@value"/>|
                          </xsl:if>
                        </xsl:for-each>
                      </xsl:attribute>
                    <xsl:attribute name="onclick">DeSelectCheck('chkBox')</xsl:attribute>
                    </xsl:element>
                  </td>
                  <xsl:for-each select="attribute[@display != 'false']">
                    <xsl:if test="@title = 'true'">
                      <xsl:choose>
                        <xsl:when test="string(@referencecolumn)">
                          <xsl:variable name="ActualColumnName" select="@name"/>
                          <xsl:variable name="referencecolumnName" select="@referencecolumn"/>
                          <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $referencecolumnName]">                            
                            <xsl:variable name="unitColumnName" select="@name"/>
                            <xsl:variable name="unitValue" select="@value"/>                            
                            <xsl:choose>
                              <xsl:when test="$unitValue = $userPreference">
                                <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $ActualColumnName]">
                                  <xsl:choose>
                                    <xsl:when test="@name = 'Quality'">
                                      <xsl:call-template name="colorCode">
                                        <xsl:with-param name="value" select="@value"/>
                                      </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>                                        
                                        <xsl:when test="string(@value)">
                                                <xsl:choose>
                                              <xsl:when test="@type = 'number'">
                                                <td style="text-align:right;">
                                                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                </td>
                                              </xsl:when>
                                              <xsl:otherwise>
                                                <td>
                                                  <xsl:choose>
                                                    <xsl:when test="@type = 'date'">
                                                      <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                                    </xsl:when>
                                                    <xsl:otherwise>                                                   
                                                      <xsl:call-template name="AddTVDSSCell">
                                                      <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                                      </xsl:call-template>
                                                    </xsl:otherwise>
                                                  </xsl:choose>
                                                </td>
                                              </xsl:otherwise>
                                            </xsl:choose>        
                                        </xsl:when>
                                        <xsl:otherwise>
                                <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                 &#160;</xsl:element>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $ActualColumnName]">
                                  <xsl:choose>
                                    <xsl:when test="@name = 'Quality'">
                                      <xsl:call-template name="colorCode">
                                        <xsl:with-param name="value" select="@value"/>
                                      </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="string(@value)">
                                          
                                          
                                          <td style="text-align:right;">
                                            <xsl:if test="$userPreference = 'metres'">
                                              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>                                              
                                            </xsl:if>
                                            <xsl:if test="$userPreference = 'feet'">
                                              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                          </td>
                                        </xsl:when>
                                        <xsl:otherwise>                                          
                                  <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                 &#160;</xsl:element>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:for-each>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@referencecolumn"/>
                          <xsl:choose>
                            <xsl:when test="@name = 'Quality'">
                              <xsl:call-template name="colorCode">
                                <xsl:with-param name="value" select="@value"/>
                              </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:choose>
                                <xsl:when test="string(@value)">
                                  <xsl:choose>
                                    <xsl:when test="@type = 'number'">
                                      <td style="text-align:right;">
                                        <xsl:value-of select="format-number(@value, '#0.00')"/>
                                      </td>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <td>
                                        <xsl:choose>
                                          <xsl:when test="@type = 'date'">
                                            <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                          </xsl:when>
                                          <xsl:otherwise>                                          
                                             <xsl:call-template name="AddTVDSSCell">
                                                      <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                                      </xsl:call-template>

                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </td>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                 &#160;</xsl:element>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                  </xsl:for-each>
                  <xsl:for-each select="attribute[@display != 'false']">
                    <xsl:if test="@title != 'true'">
                      <xsl:choose>
                        <xsl:when test="string(@referencecolumn)">
                          <xsl:variable name="ActualColumnName" select="@name"/>
                          <xsl:variable name="referencecolumnName" select="@referencecolumn"/>                              
                          <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $referencecolumnName]">                            
                            <xsl:variable name="unitColumnName" select="@name"/>
                            
                            <xsl:variable name="unitValue" select="@value"/>                            
                            <xsl:choose>
                              <xsl:when test="$unitValue = $userPreference">
                                <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $ActualColumnName]">                                  
                                  <xsl:choose>
                                    <xsl:when test="@name = 'Quality'">
                                      <xsl:call-template name="colorCode">
                                        <xsl:with-param name="value" select="@value"/>
                                      </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="string(@value)">
                                           <xsl:choose>
                                            <xsl:when test="@type = 'number'">
                                              <td style="text-align:right;">
                                                <xsl:value-of select="format-number(@value, '#0.00')"/>
                                              </td>
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <td>
                                                <xsl:choose>
                                                  <xsl:when test="@type = 'date'">
                                                    <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                                  </xsl:when>
                                                  <xsl:otherwise>
                                                    
                                                     <xsl:call-template name="AddTVDSSCell">
                                                      <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                                      </xsl:call-template>

                                                  </xsl:otherwise>
                                                </xsl:choose>
                                              </td>
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>
                               <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                &#160;</xsl:element>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $ActualColumnName]">                                  
                                  <xsl:choose>
                                    <xsl:when test="@name = 'Quality'">
                                      <xsl:call-template name="colorCode">
                                        <xsl:with-param name="value" select="@value"/>
                                      </xsl:call-template>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="string(@value)">                                          
                                         <td style="text-align:right;">
                                            <xsl:if test="$userPreference = 'metres'">
                                              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                            <xsl:if test="$userPreference = 'feet'">
                                              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                          </td>
                                        </xsl:when>
                                        <xsl:otherwise>
                               <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                &#160;</xsl:element>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:for-each>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="@referencecolumn"/>
                          <xsl:choose>
                            <xsl:when test="@name = 'Quality'">
                              <xsl:call-template name="colorCode">
                                <xsl:with-param name="value" select="@value"/>
                              </xsl:call-template>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:choose>
                                <xsl:when test="string(@value)">
                                                         <xsl:choose>
                                    <xsl:when test="@type = 'number'">
                                      <td style="text-align:right;">
                                        <xsl:value-of select="format-number(@value, '#0.00')"/>
                                      </td>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <td>
                                        <xsl:choose>
                                          <xsl:when test="@type = 'date'">
                                            <xsl:value-of select="objDate:GetDateTime(@value)"/>
                                          </xsl:when>
                                          <xsl:otherwise>
                                           
                                             <xsl:call-template name="AddTVDSSCell">
                                                      <xsl:with-param name="currentNode" select="."></xsl:with-param>
                                                      </xsl:call-template>

                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </td>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                              <xsl:element name="td">
                                 <xsl:call-template name="HideColumn">
                       <xsl:with-param name="currentNode" select="."></xsl:with-param>
                      </xsl:call-template>
                                 &#160;</xsl:element>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                  </xsl:for-each>
                </tr>
              </xsl:if>
            </xsl:for-each>
          </tbody>         
        </table>
        <!--End of table-->
		<Script language="javascript">
      lockCol("tblSearchResults",<xsl:value-of select="$ColumnsToLock"></xsl:value-of>);
      SetAlternateColor("tblSearchResults");
      setWindowTitle('<xsl:value-of select="$windowTitle"/>');
      setSelectedMapIdentifier("tblSearchResults");
      GetTableValues("tblSearchResults");
    </Script>
      </div>
    </xsl:for-each>
  </xsl:template>
  <!--Create Paging Template-->
  <xsl:template name="Loop">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>
    <xsl:if test="$StartPagenumber &lt;= $EndPageNumber">
      <xsl:choose>
        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2)) or $StartPagenumber > ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
          <!--<xsl:value-of select="$CurrentPage"/>-->
          <xsl:if test="$StartPagenumber=$CurrentPage">
            &#160; <b>
              [ <xsl:value-of select="$StartPagenumber"/> ]
            </b> &#160;
          </xsl:if>
          <xsl:if test="$StartPagenumber!=$CurrentPage">
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPaging('</xsl:text>
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
            '<xsl:value-of select="$sortType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="$StartPagenumber"/>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:when>

        <xsl:when test="$StartPagenumber=1">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;First</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a>&#160;</xsl:text>
        </xsl:when>

        <xsl:when test="$StartPagenumber=$EndPageNumber">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;Last</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2) - 1) or $StartPagenumber > ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;...</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:call-template name="Loop">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  
  <!-- Quality check Template-->
  <xsl:template name="colorCode">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="$value >= $HighRange">
        <xsl:choose>
          <xsl:when test="string(@value)">
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>;text-align:right;
              </xsl:attribute>
              <xsl:value-of select="@value"/>&#160;
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>;text-align:right;
              </xsl:attribute>
              &#160;
            </xsl:element>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$value >= $MediumRange">
            <xsl:choose>
              <xsl:when test="string(@value)">
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$MediumRangeColor'/>;text-align:right;
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$MediumRangeColor'/>;text-align:right;
                  </xsl:attribute>
                  &#160;
                </xsl:element>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="string(@value)">
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$LowRangeColor'/>;text-align:right;
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$LowRangeColor'/>;text-align:right;
                  </xsl:attribute>
                  &#160;
                </xsl:element>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
    <xsl:template name="ApplyToolTip">
    <xsl:param name="currentNode">
    </xsl:param>
    <xsl:attribute name="title">
    
   	<xsl:if test="string-length($currentNode/@tablename)&gt;0">
   	<xsl:text>Table Name: </xsl:text><xsl:value-of select="$currentNode/@tablename"></xsl:value-of><xsl:text>&#10;</xsl:text>  
	</xsl:if>

    <xsl:if test="string-length($currentNode/@dbcolumnname)&gt;0">
    <xsl:text>Column Name: </xsl:text><xsl:value-of select="$currentNode/@dbcolumnname"></xsl:value-of><xsl:text>&#10;</xsl:text>  
    </xsl:if>

    <xsl:if test="string-length($currentNode/@description)&gt;0">     
    <xsl:text>Formula: </xsl:text><xsl:value-of select="$currentNode/@description"></xsl:value-of><xsl:text>&#10;</xsl:text>  
    </xsl:if>   
    </xsl:attribute>
    </xsl:template>
    
    <xsl:template name="AddTVDSSCell">
     <xsl:param name="currentNode"/>
     <xsl:choose>
     <xsl:when test="$currentNode/@name ='Depth References'">
      <xsl:attribute name="style">
  					display:none
  				</xsl:attribute>
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
      <xsl:attribute name="style">
  					display:none
  				</xsl:attribute>
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

</xsl:stylesheet>
