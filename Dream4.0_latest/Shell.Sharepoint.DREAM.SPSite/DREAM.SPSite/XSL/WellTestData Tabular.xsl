<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt2="urn:frontpage:internal" xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/>
  <!--Parameter declaration start -->
  <xsl:param name="userPreference" select="metres"/>
  <xsl:variable name="unitValue" select="metres"/>
  <xsl:param name="formulaValue" select="0"/>
  <xsl:param name="activeDiv"/>
  <!--Parameter declaration end -->

  <!--Template to create the Response table.-->
  <xsl:template match="/" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime">
    <!--table to display response data.-->
    <!--table to display Well header data.-->
    <div>
      <table id="tblHeader" style="border-right:1px solid #336699" cellpadding="0" cellspacing="0">
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
    </div>
    <div class="gray_embossed_tabs_r" >
      <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
          <td>
            <ul class="tabNavigation">
              <li>
                <a>
                  <span id="lblgeneraltestdata" class="generaltestdata" style="cursor:hand;" onclick="javascript:MDRTabClick('generaltestdata');">General Test Data</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lbltestanalysisdata" class="testanalysisdata" style="cursor:hand" onclick="javascript:MDRTabClick('testanalysisdata');">Test Analysis Data</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lbltestformationdata" class="testformationdata" style="cursor:hand" onclick="javascript:MDRTabClick('testformationdata');">Test Formation Data</span>
                </a>
              </li>
              <li>
                <a>
                  <span id="lbltestflowdata" class="testflowdata" style="cursor:hand" onclick="javascript:MDRTabClick('testflowdata');">Test Flow Data</span>
                </a>
              </li>

              <li>
                <a>
                  <span id="lbltestintervaldata" class="testintervaldata" style="cursor:hand" onclick="javascript:MDRTabClick('testintervaldata');">Test Interval Data</span>
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
          <!--<input type="checkbox" id="chkgeneraltestdata" name="Element in Run Data">Element in Run Data</input>-->
          <div id="generaltestdata" stlye="width:100%">
            
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='generaltestdata'])&gt;0">
                <!--<xsl:text>General Test Data</xsl:text>-->
                <div class="tableContainer" id="generaltestdata">
                  <table id="tblgeneraltestdata" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
                    <tr id="trElementInRunData">
                      <xsl:variable name="tdcolspan" select="count(response/report/record/hierarchy/hierarchy[@name='generaltestdata']/record[1]/attribute[@display='true']) + count(response/report/record/hierarchy/hierarchy[@name='generaltestdata']/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])"></xsl:variable>
                      <td id="tdCheckBox" style="border-bottom:1px solid #336699;">
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="$tdcolspan"/>
                        </xsl:attribute>
                        <input type="checkbox" id="chkgeneraltestdata" name="Element in Run Data" onclick="javascript:ShowHideWellTestColumnGroups('chkgeneraltestdata','tblgeneraltestdata');">Element in Run Data</input>
                      
                      </td>
                    </tr>                   
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='generaltestdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='generaltestdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='generaltestdata']"/>
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
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tblgeneraltestdata&apos;);
          </script>

          <div id="testanalysisdata">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='testanalysisdata'])&gt;0">
               
                <div id="testanalysisdata" class="tableContainer">
                  <table id="tbltestanalysisdata"  class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:variable name="tdcolspan" select="count(response/report/record/hierarchy/hierarchy[@name='testanalysisdata']/record[1]/attribute[@display='true']) + count(response/report/record/hierarchy/hierarchy[@name='testanalysisdata']/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])"></xsl:variable>
                      <td id="tdCheckBox" style="border-bottom:1px solid #336699;">
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="$tdcolspan"/>
                        </xsl:attribute>
                        <input type="checkbox" id="chktestanalysisdata" name="Element in Run Data" onclick="javascript:ShowHideWellTestColumnGroups('chktestanalysisdata','tbltestanalysisdata');">Element in Run Data</input>
                      </td>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testanalysisdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testanalysisdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testanalysisdata']"/>
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
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tbltestanalysisdata&apos;);
            RemoveDuplicateRows(&apos;tbltestanalysisdata&apos;);
          </script>
          <div id="testformationdata">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='TestFormation'])&gt;0">                
                <div id="testformationdata" class="tableContainer">
                  <table id="tbltestformationdata" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:variable name="tdcolspan" select="count(response/report/record/hierarchy/hierarchy[@name='TestFormation']/record[1]/attribute[@display='true']) + count(response/report/record/hierarchy/hierarchy[@name='TestFormation']/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])"></xsl:variable>
                      <td id="tdCheckBox" style="border-bottom:1px solid #336699;">
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="$tdcolspan"/>
                        </xsl:attribute>
                        <input type="checkbox" id="chktestformationdata" name="Element in Run Data" onclick="javascript:ShowHideWellTestColumnGroups('chktestformationdata','tbltestformationdata');">Element in Run Data</input>
                      </td>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestFormation']"/>
                      </xsl:call-template>
                    </tr>

                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestFormation']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestFormation']"/>
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
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tbltestformationdata&apos;);
            RemoveDuplicateRows(&apos;tbltestformationdata&apos;);
          </script>

          <div id="testflowdata">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='testflowdata'])&gt;0">              
                <div class="tableContainer">
                  <table id="tbltestflowdata" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:variable name="tdcolspan" select="count(response/report/record/hierarchy/hierarchy[@name='testflowdata']/record[1]/attribute[@display='true']) + count(response/report/record/hierarchy/hierarchy[@name='testflowdata']/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])"></xsl:variable>
                      <td id="tdCheckBox" style="border-bottom:1px solid #336699;">
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="$tdcolspan"/>
                        </xsl:attribute>
                        <input type="checkbox" id="chktestflowdata" name="Element in Run Data" onclick="javascript:ShowHideWellTestColumnGroups('chktestflowdata','tbltestflowdata');">Element in Run Data</input>
                      </td>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testflowdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testflowdata']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='testflowdata']"/>
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
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tbltestflowdata&apos;);
          </script>

          <div id="testintervaldata">
            <xsl:choose>
              <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='TestInterval'])&gt;0">
                <div id="testintervaldata" class="tableContainer">
                  <table id="tbltestintervaldata" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
                    <tr>
                      <xsl:variable name="tdcolspan" select="count(response/report/record/hierarchy/hierarchy[@name='TestInterval']/record[1]/attribute[@display='true']) + count(response/report/record/hierarchy/hierarchy[@name='TestInterval']/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])"></xsl:variable>
                      <td id="tdCheckBox" style="border-bottom:1px solid #336699;">
                        <xsl:attribute name="colspan">
                          <xsl:value-of select="$tdcolspan"/>
                        </xsl:attribute>
                        <input type="checkbox" id="chktestintervaldata" name="Element in Run Data" onclick="javascript:ShowHideWellTestColumnGroups('chktestintervaldata','tbltestintervaldata');">Element in Run Data</input>
                      </td>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="isTopRow" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestInterval']"/>
                      </xsl:call-template>
                    </tr>
                    <tr>
                      <xsl:call-template name="CreateHeaderRow">
                        <xsl:with-param name="isData" select="'true'"/>
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestInterval']"/>
                      </xsl:call-template>
                    </tr>
                    <tbody class="scrollContent" id="scrollContent">
                      <xsl:call-template name="CreateDataRows">
                        <xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='TestInterval']"/>
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
          <script type="text/javascript">
            FillUpEmptyCells(&apos;tbltestintervaldata&apos;);
            RemoveDuplicateRows(&apos;tbltestintervaldata&apos;);
          </script>
         
        </td>
      </tr>
    </table>
    <script type="text/javascript">
      MDRTabClick(&apos;<xsl:value-of select="$activeDiv"/>&apos;);
      AddLastSelectedValue();
    </script>
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
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='1'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='2'">          
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="parent::*/@level='3'">
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*/parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="parent::*/parent::*"/>
              <xsl:with-param name="group" select="parent::*/parent::*/parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/parent::*/parent::*/@level"/>
            </xsl:call-template>
            <xsl:call-template name="AddCells">
              <xsl:with-param name="record" select="."/>
              <xsl:with-param name="group" select="parent::*/@name"/>
              <xsl:with-param name="level" select="parent::*/@level"/>
            </xsl:call-template>
          </xsl:if>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
 
  <xsl:template name="AddCells">
    <xsl:param name="record"/>
    <xsl:param name="group"/>
    <xsl:param name="level"/>
    <xsl:variable name="level1" select="'TestFormation'"/>
    <xsl:for-each select="$record/attribute[@display='true']">
      <td>
        <xsl:attribute name="group">
          <xsl:choose>
            <xsl:when test="$group='testanalysisdata'">
              <xsl:value-of select="$level1"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$group"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:if test="$level='2' or $level='3'">
          <xsl:attribute name="style">
            display:none
          </xsl:attribute>
        </xsl:if>
        <xsl:if test ="@type='Number'">
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
              <xsl:value-of select="format-number((@value div $formulaValue), '#0.00')"/>
            </xsl:when>
            <xsl:when test="(string-length(@referencecolumn)&gt;0) and ($userPreference = 'feet')">
              <xsl:value-of select="format-number((@value * $formulaValue), '#0.00')"/>
            </xsl:when>
            <xsl:when test ="@type='Number'">
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
    <xsl:param name="isTopRow"/>
    <xsl:variable name="level1" select="'Strings'"/>
    <xsl:variable name="level2" select="'ElementInRunData'"/>
    <xsl:variable name="colCount" select="0"></xsl:variable>
    <xsl:for-each select="$childColl/record[1]/attribute[@display='true']">
      
      <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
        
          <!--<xsl:attribute name="id">
            <xsl:text>thr</xsl:text> + 
            <xsl:value-of select="@name"/>
          </xsl:attribute>-->        
        <xsl:attribute name="group">
          <xsl:value-of select="$level1"/>
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">                
            <xsl:if test="position()=count($childColl/record[1]/attribute[@display='true'])">
              <xsl:attribute name="style">
                display:block;border-right:1px solid #336699;font-weight:bold"
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="position()='1'">              
              <xsl:value-of select="$childColl/@name"></xsl:value-of>             
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <!--<xsl:text>3:</xsl:text>-->
            <xsl:attribute name="class">
              Header
            </xsl:attribute>           
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  &#160;(m)
                </xsl:when>
                <xsl:otherwise>
                  &#160;(ft)
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
      <!--<xsl:text>|</xsl:text>-->
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px; " nowrap="nowrap">
        <xsl:attribute name="group">
          <xsl:value-of select="$level2"/>
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="style">          
          display:none;border-bottom:1px solid #336699;font-weight:bold"
        </xsl:attribute>      
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">
            <xsl:if test="position()=count($childColl/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true'])">
              <!--<xsl:text>5:</xsl:text>-->
              <xsl:attribute name="style">
                display:none;font-weight:bold;border-bottom:1px solid #336699;font-weight:bold"
              </xsl:attribute>
            </xsl:if>

            <xsl:if test="position()='1'">            
              &#160;
              <xsl:value-of select="$childColl/record[hierarchy/@name][1]/hierarchy/@name"></xsl:value-of>              
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="class">
              Header
            </xsl:attribute>
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  &#160;(m)
                </xsl:when>
                <xsl:otherwise>
                  &#160;(ft)
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
      <!--<xsl:text>|</xsl:text>-->
    </xsl:for-each>
    <xsl:for-each select="$childColl/record[hierarchy/@name]/hierarchy/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
      <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
        <xsl:attribute name="group">
          <!--<xsl:value-of select="$level3"/>-->
        </xsl:attribute>
        <xsl:call-template name="AddDataType">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="ApplyToolTip">
          <xsl:with-param name="currentNode" select="."></xsl:with-param>
        </xsl:call-template>
        <xsl:attribute name="style">
          display:none;border-bottom:1px solid #336699;font-weight:bold"
        </xsl:attribute>
        <xsl:if test="$isData='false'">
          <input type="checkbox"/>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="$isTopRow='true'">
            <xsl:if test="position()='1'">
              &#160;
              <xsl:text>Wireline Retrievables</xsl:text>
            </xsl:if>
            &#160;
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="class">
              Header
            </xsl:attribute>
            <xsl:if test="$isData='true'">
              <xsl:value-of select="@name"/>
            </xsl:if>
            <xsl:if test="string-length(@referencecolumn)&gt;0">
              <xsl:variable name="refCol" select="@referencecolumn"/>
              <xsl:variable name="refUnit" select="parent::*/attribute[@name=$refCol][1]/@value"></xsl:variable>
              <xsl:choose >
                <xsl:when test="$userPreference = 'metres'">
                  &#160;(m)
                </xsl:when>
                <xsl:otherwise>
                  &#160;(ft)
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </th>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="CreateTopHeaderRow">
    <xsl:param name="childColl"/>
    <xsl:param name="isData"/>
    <xsl:param name="isTopRow"/>      
  
      <xsl:for-each select="$childColl/record[1]/attribute[@display='true']">
        <xsl:if test="$isData='true'">
          <xsl:if test="@name = 'UWI'">
            <xsl:value-of select="'UWI'"/>
            <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
              <xsl:choose>
                <xsl:when test="$isTopRow='true'">
                  <xsl:if test="position()=count($childColl/record[1]/attribute[@display='true'])">
                    <xsl:attribute name="style">
                      border-right:1px solid #336699;font-weight:bold"
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="position()='1'">
                    <xsl:value-of select="$childColl/@name"></xsl:value-of>
                  </xsl:if>
                  &#160;
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="class">
                    Header
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </th>
          </xsl:if>
          <xsl:if test="@name = 'UWBI'">
            <!--<xsl:text>$childColl/@name</xsl:text>
            <xsl:value-of select="$childColl/@name"/>-->
            <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
              <xsl:choose>
                <xsl:when test="$isTopRow='true'">
                  <xsl:if test="position()=count($childColl/record[1]/attribute[@display='true'])">
                    <xsl:attribute name="style">
                      border-right:1px solid #336699;font-weight:bold"
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="position()='1'">
                    <xsl:value-of select="$childColl/@name"></xsl:value-of>
                  </xsl:if>
                  &#160;
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="class">
                    Header
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </th>
          </xsl:if>
          <xsl:if test="@name = 'Conduit'">
            <xsl:value-of select="'Conduit'"/>
            <th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
              <xsl:choose>
                <xsl:when test="$isTopRow='true'">
                  <xsl:if test="position()=count($childColl/record[1]/attribute[@display='true'])">
                    <xsl:attribute name="style">
                      border-right:1px solid #336699;font-weight:bold"
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:if test="position()='1'">
                    <xsl:value-of select="$childColl/@name"></xsl:value-of>
                  </xsl:if>
                  &#160;
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="class">
                    Header
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </th>
          </xsl:if>
        </xsl:if>
        
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
        <!--<xsl:value-of select="$currentNode/@tablename"></xsl:value-of>-->
        <xsl:value-of select="$currentNode/@name"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

      <xsl:if test="string-length($currentNode/@dbcolumnname)&gt;0">
        <xsl:text>Column Name: </xsl:text>
        <!--<xsl:value-of select="$currentNode/@dbcolumnname"></xsl:value-of>-->
        <xsl:value-of select="$currentNode/@name"></xsl:value-of>
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

</xsl:stylesheet>