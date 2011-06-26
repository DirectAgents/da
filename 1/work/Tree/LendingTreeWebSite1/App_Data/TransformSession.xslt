<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="keys">
    <table border="1">
      <xsl:apply-templates select="key" />
    </table>
  </xsl:template>
  <xsl:template match="key">
    <tr>
      <td>
        <xsl:value-of select="@name" />
      </td>
      <td>
        <pre>
          <xsl:value-of select="." />
        </pre>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
