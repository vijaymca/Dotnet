<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:objDate="urn:DATE">
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

    <!--Looping through each report-->
    <xsl:for-each select="response/report">
      <!-- End Of Show previous/next page links-->
      <!--Creating Scrollbar.-->
      <div id="tableContainer" class="tableContainer">
        <!--table to display response data.-->
        <!--<xsl:value-of select="myObj:GetDateTime('2006-08-22T06:30:07.7199222-04:00')"/>-->        
        <table style="border-right:1px solid #bdbdbd;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
          <xsl:for-each select="record[@recordno=1]">
            <thead class="fixedHeader" id="fixedHeader">
              <tr style="height: 20px;">
                <th width="15px" text-align="center">
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
                    </xsl:element>
                  </xsl:if>
                </xsl:for-each>
                <xsl:for-each select="attribute[@display != 'false']">
                  <xsl:if test="@title != 'true'">
                    <xsl:element name="th">
                     <xsl:call-template name="AddDataType">
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
                  <td width="15px" text-align="center">
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
                            <xsl:when test="@type = 'date'">
                             <td >
                               <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@type = 'number'">
                                                <td style="text-align:right;">
                                                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                </td>
					</xsl:when>
                            <xsl:otherwise>
                            <td >
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
  							 </xsl:choose>  
                                         </xsl:when>
                                        <xsl:otherwise>
                                          <td>&#160;</td>
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
                                          <td>
                                            <xsl:if test="$userPreference = 'metres'">
                                              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>                                            
                                            </xsl:if>
                                            <xsl:if test="$userPreference = 'feet'">
                                              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                          </td>
                                        </xsl:when>
                                        <xsl:otherwise>                                          
                                          <td>&#160;</td>
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
                            <xsl:when test="@type = 'date'">
                             <td >
                               <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@type = 'number'">
                                                <td style="text-align:right;">
                                                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                </td>
					</xsl:when>
                            <xsl:otherwise>
                            <td >
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
  							 </xsl:choose>  
                                </xsl:when>
                                <xsl:otherwise>
                                  <td>&#160;</td>
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
                          <xsl:variable name="referencecolumnName" select="@referencecolumn"/>                                                 <xsl:for-each select="/response/report/record[@recordno = $recordNumber]/attribute[@name = $referencecolumnName]">                            
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
                            <xsl:when test="@type = 'date'">
                             <td >
                               <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@type = 'number'">
                                                <td style="text-align:right;">
                                                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                </td>
					</xsl:when>
                            <xsl:otherwise>
                            <td >
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
  							 </xsl:choose>  
                                        </xsl:when>
                                        <xsl:otherwise>
                                          <td>&#160;</td>
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
                                          <td>
                                            <xsl:if test="$userPreference = 'metres'">
                                              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                            <xsl:if test="$userPreference = 'feet'">
                                              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
                                            </xsl:if>
                                          </td>
                                        </xsl:when>
                                        <xsl:otherwise>
                                          <td>&#160;</td>
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
                            <xsl:when test="@type = 'date'">
                             <td >
                               <xsl:value-of select="objDate:GetDateTime(@value)"/>
                              </td>
                            </xsl:when>
                            <xsl:when test="@type = 'number'">
                                                <td style="text-align:right;">
                                                  <xsl:value-of select="format-number(@value, '#0.00')"/>
                                                </td>
					</xsl:when>
                            <xsl:otherwise>
                            <td >
                                <xsl:value-of select="@value"/>
                              </td>
                            </xsl:otherwise>
  							 </xsl:choose>  
                                </xsl:when>
                                <xsl:otherwise>
                                  <td>&#160;</td>
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
  
  <!-- Quality check Template-->
  <xsl:template name="colorCode">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="$value >= $HighRange">
        <xsl:choose>
          <xsl:when test="string(@value)">
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>
              </xsl:attribute>
              <xsl:value-of select="@value"/>&#160;
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="td">
              <xsl:attribute name="style">
                background-color:<xsl:value-of select='$HighRangeColor'/>
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
                    background-color:<xsl:value-of select='$MediumRangeColor'/>
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$MediumRangeColor'/>
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
                    background-color:<xsl:value-of select='$LowRangeColor'/>
                  </xsl:attribute>
                  <xsl:value-of select="@value"/>&#160;
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="td">
                  <xsl:attribute name="style">
                    background-color:<xsl:value-of select='$LowRangeColor'/>
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
 <xsl:template name="AddDataType">
     <xsl:param name="currentNode"/>
      <xsl:attribute name="type">
  	<xsl:value-of select="$currentNode/@type"></xsl:value-of>
  	</xsl:attribute>
    </xsl:template>

</xsl:stylesheet>
