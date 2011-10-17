/*
Copyright (c) 2009 Bill Davidsen (wdavidsen@yahoo.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing.Design;
using System.Drawing;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace SCS.Web.UI.WebControls.Design
{
    public class SplitterCssClassConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
                return true;

            return base.CanConvertFrom(context, t);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            if (destType == typeof(InstanceDescriptor) || destType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
        {
            if (value is string)
            {
                string styleClassList = (string)value;

                try
                {
                    string[] classes = styleClassList.Split(info.TextInfo.ListSeparator[0]);
                    string[] args = new string[4];

                    for (int i = 0; i < classes.Length; i++)
                    {
                        args[i] = classes[i].Trim();
                    }

                    return new SplitterCssClasses(args[0], args[1], args[2], args[3]);
                }
                catch
                {
                    return new SplitterCssClasses();
                }
            }
            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type destType)
        {
            if (destType == typeof(string))
            {
                SplitterCssClasses style = (SplitterCssClasses)value;

                string text = string.Empty;

                if (!string.IsNullOrEmpty(style.ResizableAreaCssClass))
                    text += string.Format("{0} {1}",
                        info.TextInfo.ListSeparator[0], style.ResizableAreaCssClass);

                if (!string.IsNullOrEmpty(style.LeftPaneCssClass))
                    text += string.Format("{0} {1}",
                        info.TextInfo.ListSeparator[0], style.LeftPaneCssClass);

                if (!string.IsNullOrEmpty(style.DividerCssClass))
                    text += string.Format("{0} {1}",
                        info.TextInfo.ListSeparator[0], style.DividerCssClass);

                if (!string.IsNullOrEmpty(style.RightPaneCssClass))
                    text += string.Format("{0} {1}",
                        info.TextInfo.ListSeparator[0], style.RightPaneCssClass);

                if (text.Length > 0)
                    text = text.Substring(2);

                return text;
            }
            else if (destType == typeof(InstanceDescriptor))
            {
                Type[] types = new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) };
                ConstructorInfo constructorInfo = typeof(SplitterCssClasses).GetConstructor(types);

                SplitterCssClasses style = (SplitterCssClasses)value;
                object[] args = new object[] { 
                    style.ResizableAreaCssClass, 
                    style.LeftPaneCssClass, 
                    style.DividerCssClass, 
                    style.RightPaneCssClass };

                InstanceDescriptor descriptor = new InstanceDescriptor(constructorInfo, args, true);

                return descriptor;
            }
            return base.ConvertTo(context, info, value, destType);
        }
    }
}