<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="LendingTreeAffiliateRequest">
    <table>
      <tr>
        <xsl:apply-templates select="Request" />
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="Request">
    <td>
      <xsl:value-of select="@type" />
    </td>
  </xsl:template>
</xsl:stylesheet>