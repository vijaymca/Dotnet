<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output  method="xml" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <html>
      <head>
        <link rel="Stylesheet" href="/_LAYOUTS/DREAM/Styles/DetailReportRel2_1.css" type="text/css" />
      </head>

      <body>
        <!--table to display User Details.-->
        <table id="tblDataOwner" cellspacing="0" cellpadding="0" class="tblDataOwner">
          <tr >
            <th style="border-right:1px solid #9B9797;" class="thDataOwner">Data Owner Name</th>
            <th class="thDataOwner">Contact Details</th>
          </tr>
          <xsl:for-each select="Users/User">
            <xsl:sort select="@dataSource" data-type="text" order="ascending"/>
            <tr >
              <xsl:choose>
                <xsl:when test="string-length(string(@displayName))&gt;0">
                  <td valign="top" class="tdDataOwnerNames">
                    <xsl:value-of select="@displayName"/>
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="tdDataOwnerContactDetails">&#160;</td>
                </xsl:otherwise>
              </xsl:choose>
              <td>
                <table width="100%" cellspacing="0" cellpadding="0">
                  <tr>
                    <td valign="top" class="tdDataOwnerContactDetails" width="40%">
                      Data Source
                    </td>
                    <xsl:choose>
                      <xsl:when test="string-length(string(@dataSource))&gt;0">
                        <td class="tdDataOwnerContactDetails" width="60%">
                          <xsl:value-of select="@dataSource"/>
                        </td>
                      </xsl:when>
                      <xsl:otherwise>
                        <td class="tdDataOwnerContactDetails" width="60%">&#160;</td>
                      </xsl:otherwise>
                    </xsl:choose>
                  </tr>
                  <xsl:for-each select="*">
                    <tr >
                      <xsl:choose>
                        <xsl:when test="string-length(string(@name))&gt;0">
                          <td valign="top" class="tdDataOwnerContactDetails" width="40%">
                            <xsl:value-of select="@name"/>
                          </td>
                        </xsl:when>
                        <xsl:otherwise>
                          <td valign="top" class="tdDataOwnerContactDetails" width="40%">&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:choose>
                        <xsl:when test="string-length(string(@name))&gt;0">
                          <td class="tdDataOwnerContactDetails" width="60%">
                            <xsl:for-each select="*">
                              <xsl:choose>
                                <xsl:when test="@name = 'Telephone'">
                                  <xsl:value-of select="."/>
                                </xsl:when>
                                <xsl:when test="@name = 'Extension'">
                                  &#x2002;&#x2002;Extn:  <xsl:value-of select="."/>
                                  <br/>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="."/>
                                  <br/>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:for-each>
                          </td>
                        </xsl:when>
                        <xsl:otherwise>
                          <td valign="top" class="tdDataOwnerContactDetails" width="60%">&#160;</td>
                        </xsl:otherwise>
                      </xsl:choose>
                    </tr>
                  </xsl:for-each>
                </table>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <Script language="javascript">
          setWindowTitle('Data Owner Details');
        </Script>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>