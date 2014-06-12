<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:ddwrt2="urn:frontpage:internal" xmlns:objDate="urn:DATE">
  <xsl:output method="xml"/> 
  <!--Parameter declaration start --> 
  
  <xsl:param name="recordsPerPage" select="0" />
  <!-- Page Number field -->
  <xsl:param name="pageNumber" select="1" />
  <!--Record Count Field-->
  <xsl:param name="recordCount" select="0"/>
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
  <xsl:param name="activeDiv"/>  
  <!--End of parameter declaration -->
  <!--Variable Declaration -->
  <xsl:variable name="counter" select="1"/>
  <!--Variable declared to terminate the column header generator loop-->
    <xsl:variable name="flag" select="0"/>
     <xsl:variable name="totalRecord" select="0"/>
    <xsl:variable name="Pages" select="0" />
    <!--To do paging with page numbers-->
    <xsl:variable name="StartPagenumber" select="0"/>  
    <!--Setting start and end count -->
                <xsl:variable name="startCount" select="($recordsPerPage * ($pageNumber)) + 1" />
                 <xsl:variable name="endCount" select="$recordsPerPage * ($pageNumber + 1)" />    
                <!--End of Setting start and end count -->
   <!--End of Variable Declaration -->

  
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
                    <span id="lblholesection" class="holesection" style="cursor:hand;" onclick="javascript:MDRTabClick('holesection');">Hole Sections</span>
                  </a>
                </li>
                <li>
                  <a>
                   <span id="lblcasings" class="casings" style="cursor:hand" onclick="javascript:MDRTabClick('casings');">Casings</span>
                  </a>
                </li>
                 <li>
                  <a>
                   <span id="lblliners" class="liners" style="cursor:hand" onclick="javascript:MDRTabClick('liners');">Liners</span>
                  </a>
                </li>
                 <li>
                  <a>
                   <span id="lblmechanicalcontent" class="mechanicalcontent" style="cursor:hand" onclick="javascript:MDRTabClick('mechanicalcontent');">Mechanical Content</span>                  </a>
                </li>
                <li>
                  <a>
                   <span id="lblfluidscements" class="fluidscements" style="cursor:hand" onclick="javascript:MDRTabClick('fluidscements');">Wellbore &amp; Annuli</span>                  </a>
                </li>
				<li>
                  <a>
                   <span id="lblgrossperforations" class="grossperforations" style="cursor:hand" onclick="javascript:MDRTabClick('grossperforations');">Gross Perforations</span>                  </a>
                </li>
				<li>
                  <a>
                   <span id="lblwellhead" class="wellhead" style="cursor:hand" onclick="javascript:MDRTabClick('wellhead');">Well Head</span>                  </a>
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
	<tr><td id="parentCell">
		
        
	<div id="holesection" stlye="width:100%">
	 <xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Hole Section'])&gt;0">
	 <!--<table class="hidePrintLink" ><tr><td>
	  <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'holesection'"/>
    <xsl:with-param name="nodeName" select="'Hole Section'"/>
	  </xsl:call-template>
	 </td></tr></table>-->
	  <div class="tableContainer">		
	  <table id="tblHole" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Hole Section']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Hole Section']"/>
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
     	FillUpEmptyCells(&apos;tblHole&apos;);
     </script>
      
	<div id="casings">
	<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Casings'])&gt;0">
	 <table class="hidePrintLink">
	<!-- <tr><td>
	 <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'casings'"/>
    <xsl:with-param name="nodeName" select="'Casings'"/>
	  </xsl:call-template>
	 </td></tr>	-->
		<tr>
			<td align="right">
				<input id="chkParentCasings" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentCasings','chkChildCasings','ComponentInfo','WirelineRetrievables','tblCasings');"/>Component Info
				&#160;
				<input id="chkChildCasings" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentCasings','chkChildCasings','ComponentInfo','WirelineRetrievables','tblCasings');"/>Wireline Retrievables

			</td>
		</tr>
	</table>
	

	<div id="casings" class="tableContainer">
	<table id="tblCasings"  class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
	
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
        			<xsl:with-param name="isTopRow" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>          			
          		</xsl:call-template> 
        	</tr>  
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>          			
          		</xsl:call-template> 
        	</tr>          	      	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Casings']"/>
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
     	FillUpEmptyCells(&apos;tblCasings&apos;);
     	RemoveDuplicateRows(&apos;tblCasings&apos;);
     </script>
	<div id="liners">
	<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Liners'])&gt;0">
	 
	 <table class="hidePrintLink" >
	<!-- <tr><td>
	 <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'liners'"/>
    <xsl:with-param name="nodeName" select="'Liners'"/>
	  </xsl:call-template>
	 </td></tr>	-->
		<tr>
			<td align="right">
				<input id="chkParentLiners" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentLiners','chkChildLiners','ComponentInfo','WirelineRetrievables','tblLiners');"/>Component Info
				&#160;
				<input id="chkChildLiners" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentLiners','chkChildLiners','ComponentInfo','WirelineRetrievables','tblLiners');"/>Wireline Retrievables

			</td>
		</tr>
	</table>
	<div id="liners" class="tableContainer">
	<table id="tblLiners" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
        			<xsl:with-param name="isTopRow" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>          			
          		</xsl:call-template> 
        	</tr>  

        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Liners']"/>
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
     	FillUpEmptyCells(&apos;tblLiners&apos;);
     	RemoveDuplicateRows(&apos;tblLiners&apos;);
     </script> 
     
<div id="mechanicalcontent">
<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Mechanical Content'])&gt;0">
	  <!-- <table class="hidePrintLink" ><tr><td>
	  <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'mechanicalcontent'"/>
    <xsl:with-param name="nodeName" select="'Mechanical Content'"/>
	  </xsl:call-template>
	 </td></tr></table>-->
	  <div class="tableContainer">	    
	  	  <table id="tblMechanicalcontent" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Mechanical Content']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Mechanical Content']"/>
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
     	FillUpEmptyCells(&apos;tblMechanicalcontent&apos;);
     </script> 
	<div id="fluidscements">
	<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli'])&gt;0">
	 <!--  <table class="hidePrintLink"><tr><td>
	  <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'fluidscements'"/>
    <xsl:with-param name="nodeName" select="'Wellbore Annuli'"/>
	  </xsl:call-template>
	 </td></tr></table>-->
	 <div class="tableContainer">	    
	  <table id="tblFluidscements" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellbore Annuli']"/>
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
     	FillUpEmptyCells(&apos;tblFluidscements&apos;);
     </script>  
	<div id="grossperforations">
	<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Gross Perforations'])&gt;0">
	 <table class="hidePrintLink">
	 <!--<tr><td>
	 <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'grossperforations'"/>
    <xsl:with-param name="nodeName" select="'Gross Perforations'"/>
	  </xsl:call-template>
	 </td></tr>-->
		<tr>
			<td>
				<input id="chkParentGrossperforations" type="checkbox" onclick="javascript:ShowHideColumnGroups('chkParentGrossperforations','chkChildGrossperforations','ComponentInfo','WirelineRetrievables','tblGrossperforations');"/>Perforation Intervals			
					&#160;
				<input id="chkChildGrossperforations" type="checkbox" style="display:none" onclick="javascript:ShowHideColumnGroups('chkParentGrossperforations','chkChildGrossperforations','ComponentInfo','WirelineRetrievables','tblGrossperforations');"/><span style="display:none">NetPerforations</span>
			</td>
		</tr>
	</table>
<div id="grossperforations" class="tableContainer">
	  <table id="tblGrossperforations" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
	  <tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
        			<xsl:with-param name="isTopRow" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>          			
          		</xsl:call-template> 
        	</tr>  

        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Gross Perforations']"/>
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
     	FillUpEmptyCells(&apos;tblGrossperforations&apos;);
     	RemoveDuplicateRows(&apos;tblGrossperforations&apos;);
     </script>  
	<div id="wellhead">
	<xsl:choose>
	  <xsl:when test="count(response/report/record/hierarchy/hierarchy[@name='Wellhead'])&gt;0">
	 <!--  <table class="hidePrintLink"><tr><td>
	  <xsl:call-template name="Paging">	  
	  <xsl:with-param name="divName" select="'wellhead'"/>
    <xsl:with-param name="nodeName" select="'Wellhead'"/>
	  </xsl:call-template>
	 </td></tr></table>-->
	 <div class="tableContainer">	
	 	  <table id="tblWellhead" class="scrollTable" style="border-right:1px solid #336699;width:100%" cellpadding="0" cellspacing="0">
        	<tr>
        		<xsl:call-template name="CreateHeaderRow">
        			<xsl:with-param name="isData" select="'true'"/>
          			<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellhead']"/>          			
          		</xsl:call-template> 
        	</tr>        	
        <tbody class="scrollContent" id="scrollContent">        	
        	<xsl:call-template name="CreateDataRows">
        		<xsl:with-param name="childColl" select="response/report/record/hierarchy/hierarchy[@name='Wellhead']"/>
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
     	FillUpEmptyCells(&apos;tblWellhead&apos;);
     </script> 
  </td></tr>
     </table>
     <script type="text/javascript">
     	MDRTabClick(&apos;<xsl:value-of select="$activeDiv"/>&apos;);
     	AddLastSelectedValue();
     </script> </xsl:template>
     
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
  	<xsl:variable name="level1" select="'Strings'"/>  
  	<xsl:for-each select="$record/attribute[@display='true']">
  			<td>
  				<xsl:attribute name="group">
  				<xsl:choose>
  				<xsl:when test="$group='Casings'">
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
  	<xsl:variable name="level2" select="'ComponentInfo'"/>
	<xsl:variable name="level3" select="'WirelineRetrievables'"/>
  		<xsl:for-each select="$childColl/record[1]/attribute[@display='true']">
  			<th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
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
  			<xsl:attribute name="style">
  					border-right:1px solid #336699;font-weight:bold;border-bottom:1px solid #336699;font-weight:bold"
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
		</xsl:for-each>
		<xsl:for-each select="$childColl/record[hierarchy/@name]/hierarchy/record[hierarchy/@name][1]/hierarchy/record[1]/attribute[@display='true']">
  			<th  style="border-top:1px solid #336699;font-weight:bold;padding:4px;" nowrap="nowrap">
  				<xsl:attribute name="group">
  					<xsl:value-of select="$level3"/>
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
  <xsl:template name="Message">
   <xsl:text>There is no item to show in this view.</xsl:text>
  </xsl:template>
  
          <xsl:template name="RecordCountDisplay">
    <xsl:param name="recordCount" />
    <xsl:param name="startValue"/>
     <xsl:param name="currentPageNo"/>      
      <xsl:variable name="endValueTemp" select="$recordsPerPage * (($currentPageNo - 1) + 1)" />
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
        <xsl:variable name="endValue" select="$recordsPerPage * (($currentPageNo - 1) + 1)" />
        <b>
          <xsl:value-of select="$endValue" />
        </b>
      </xsl:if>
      <xsl:if test="$endValueTemp &gt;= $recordCount">
        <xsl:variable name="endValue" select="$recordCount" />
        <b>
          <xsl:value-of select="$endValue" />
        </b>
      </xsl:if>
      shown. &#160; &#160; &#160; &#160;   
      <!-- End Of Show previous/next page links-->     
    </xsl:template>
    
      <!--Create Paging Template-->
  <xsl:template name="Loop">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>
    <xsl:param name="recordCount"/>
    <xsl:param name="currentPageNo"/>    
    <xsl:if test="$StartPagenumber &lt;= $EndPageNumber">
      <xsl:choose>
        <xsl:when test="($StartPagenumber &gt; ($currentPageNo - ceiling($MaxPages div 2)) or $StartPagenumber &gt; ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $currentPageNo + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
          <!--<xsl:value-of select="$CurrentPage"/>-->
          <xsl:if test="$StartPagenumber=$currentPageNo">
            &#160; <b>
              [ <xsl:value-of select="$StartPagenumber"/> ]
            </b> &#160;
          </xsl:if>
          <xsl:if test="$StartPagenumber!=$currentPageNo">
            <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:MDRReportPaging('</xsl:text>
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
            '<xsl:value-of select="$sortType"/>',this.parentElement.parentElement.parentElement.parentElement.parentElement.id)"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="$StartPagenumber"/>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:when>

        <xsl:when test="$StartPagenumber=1">
          <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:MDRReportPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>',this.parentElement.parentElement.parentElement.parentElement.parentElement.id)"
          <xsl:text disable-output-escaping="yes">&gt;First</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a>&#160;</xsl:text>
        </xsl:when>

        <xsl:when test="$StartPagenumber=$EndPageNumber">
          <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:MDRReportPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>',this.parentElement.parentElement.parentElement.parentElement.parentElement.id)"
          <xsl:text disable-output-escaping="yes">&gt;Last</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber &gt; ($currentPageNo - ceiling($MaxPages div 2) - 1) or $StartPagenumber &gt; ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $currentPageNo + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
          <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:MDRReportPaging('</xsl:text>
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
          '<xsl:value-of select="$sortType"/>',this.parentElement.parentElement.parentElement.parentElement.parentElement.id)"
          <xsl:text disable-output-escaping="yes">&gt;...</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:call-template name="Loop">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
        <xsl:with-param name="recordCount" select="$recordCount"/>
         <xsl:with-param name="currentPageNo" select="$currentPageNo"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="Paging">
  <xsl:param name="divName"></xsl:param>
   <xsl:param name="nodeName"></xsl:param>

	<xsl:variable name="totalRecord" select="count(response/report/record/hierarchy/hierarchy[@name=$nodeName]/record)"></xsl:variable>
        <xsl:variable name="Pages" select="ceiling($totalRecord div $recordsPerPage)" />
	  <!--for paging     -->     
   <!-- Call Templates of for page number Loop-->  
 <xsl:choose>
    <xsl:when test="$activeDiv=$divName">
    <xsl:call-template name="Loop">
      <xsl:with-param name="EndPageNumber" select="$Pages"/>
      <xsl:with-param name="StartPagenumber" select="1"/>  
       <xsl:with-param name="recordCount" select="$totalRecord"/>  
        <xsl:with-param name="currentPageNo" select="$CurrentPage"/>   
    </xsl:call-template>
   <br></br>
       <xsl:call-template name="RecordCountDisplay">   
    <xsl:with-param name="recordCount" select="$totalRecord"/>
     <xsl:with-param name="startValue" select="$startCount"/>
      <xsl:with-param name="currentPageNo" select="$CurrentPage"/>  
    </xsl:call-template>
    </xsl:when>
    <xsl:otherwise>
    <xsl:call-template name="Loop">
      <xsl:with-param name="EndPageNumber" select="$Pages"/>
      <xsl:with-param name="StartPagenumber" select="1"/>  
       <xsl:with-param name="recordCount" select="$totalRecord"/>  
        <xsl:with-param name="currentPageNo" select="1"/>   
    </xsl:call-template>
     <br></br>
     <xsl:call-template name="RecordCountDisplay">   
    <xsl:with-param name="recordCount" select="$totalRecord"/>
     <xsl:with-param name="startValue" select="1"/>
      <xsl:with-param name="currentPageNo" select="1"/>   
    </xsl:call-template>
    </xsl:otherwise>
    </xsl:choose>
    <!-- End Call Templates of for page number Loop-->
   
      <!--end paging     -->	 

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