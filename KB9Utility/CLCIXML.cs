using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

/************************************************************************/
/* 
 * read /write xml file, parse its data.
 * */
/************************************************************************/
namespace KB9Utility
{
    public class CLCIXML
    {
        private XmlDocument m_doc=null;
        private XmlElement m_current = null;
        private XmlElement m_root = null;
        private XmlElement m_saved = null;

        public CLCIXML()
        {

            m_doc = new XmlDocument();
            
        }
        ~CLCIXML()
        {

        }
        private void init()
        {
            m_doc = new XmlDocument();

        }

        /************************************************************************/
        /* 
         * delete a attribute from the element
         * */
        /************************************************************************/
        public  bool del_attribute(string name)
        {
            m_current.RemoveAttribute(name);
            return true;
        }
        /************************************************************************/
        /* get attribute value.
         * The caller get_attribute(name, ref value). //must has this "ref"
         * 
         * comments: 
         *  ref: parameters needs to assign initial value.
         *  out: no initial value, but it must been assign value in implemented function.
         * 
         * */
        /************************************************************************/
        public bool get_attribute(string name, ref string value)
        {
            if (m_current.HasAttribute(name))
                value = m_current.GetAttribute(name);
            else
                return false;
            return true;

        }
        /************************************************************************/
        /* 
         * create a new attribute in current element.
         * */
        /************************************************************************/
        public bool new_attribute(string name, string value)
        {
            m_current.SetAttribute(name, value);
            return true;
        }
        /************************************************************************/
        /* 
         * set new value to existed attribute.
         * */
        /************************************************************************/
        public bool set_attribute(string name, string value)
        {
            m_current.SetAttribute(name, value);
            return true;
        }
        /************************************************************************/
        /* find next group from current group. next sliding.
         * */
        /************************************************************************/
        public bool get_next_group(string name)
        {
            XmlNode node;
            XmlNode element = m_current;
            do 
            {
                node = element.NextSibling;
                if (node == null) return false;
                if (!(node is XmlElement)) 
                {
                    element = node;
                }
                if (node.Name == name)
                {
                    m_current = (XmlElement)node;
                    return true;
                }
                element = node;
                node = null;


            } while (node != null);
            return false;
            
        }
        public bool del_group(string name)
        {
            XmlNode node = find_first_node(m_current, name);
            if (node == null) return true;
            m_current.RemoveChild(node);
            return true;
            
        }
        public bool get_first_group(string name)
        {
            XmlNode node = find_first_node(m_current, name);
            if (node == null) return false;
            if (!(node is XmlElement)) return false;
            m_current = (XmlElement)node;
            return true;
        }
        public bool get_current_group_value(ref string value)
        {
            value = m_current.InnerText;
            return true;
        }
        public bool new_group(string name, bool bcurrent)
        {
            /*
            XmlElement element = m_doc.CreateElement(name);
            XmlNode node = m_current.AppendChild(element);
            if (bcurrent)
                m_current = (XmlElement)node;
            return true;
             * */
            XmlNode node = m_doc.CreateElement(name);
            if (m_current != null)
                node = m_current.AppendChild(node);
            else
            {
                if (m_root != null)
                    node = m_root.AppendChild(node);
                else
                {
                    if (m_doc != null)
                    {
                        m_root = (XmlElement)(m_doc.AppendChild(node));
                        m_current = m_root;
                        node = m_root;
                    }
                    else
                        return false;
                }
            }
            if (bcurrent)
                m_current = (XmlElement)node;
            return true;
        }
        public bool new_group(string name, string value, bool bcurrent)
        {
            XmlElement element = m_doc.CreateElement(name);
            element.InnerText = value;
            XmlNode node = m_current.AppendChild(element);
            if (bcurrent)
                m_current = (XmlElement)node;
            return true;
        }
        public bool set_group(string name, string value)
        {
            //XmlNode node = find_first_node(m_current, name);
            if (m_current == null) return false;
            //if (!(node is XmlElement)) return false;
           // XmlElement element = (XmlElement)node;
            m_current.InnerText = value;
            return true;

        }
        public bool load_string(string txtXml)
        {
            
            try
            {
                m_doc.LoadXml(txtXml);
                if (m_doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    XmlNode node = m_doc.FirstChild;
                    m_root = (XmlElement)node.NextSibling;
                    
                }
                else
                {
                    m_root = (XmlElement)(m_doc.FirstChild);
                }
                m_current = m_root;
            }
            catch (System.Exception ex)
            {
                m_root = null;
                m_current = null;
                m_saved = null;
            }

            return true;

        }
        public string get_xml_string()
        {

            return m_doc.InnerXml;

        }
        public string get_root_string()
        {
            if (m_root == null) return "";
            return m_root.InnerXml;
        }
        public bool close()
        {

            m_doc.RemoveAll();
            m_doc = null;
            m_root = null;
            m_current = null;
            m_saved = null;
            return true;
        }
        public bool new_doc_with_root(string strRoot)
        {

         
            System.Xml.XmlElement element = this.m_doc.CreateElement(strRoot);

            XmlElement node = (XmlElement)(this.m_doc.AppendChild(element));
            m_root = node;
            m_current = m_root;
            return true;

        }
        public bool open_file(string filename, string rootname, bool bcreate)
        {
            if (!File.Exists(filename))
            {
                if (!bcreate) return false;
                string s = string.Format("<{0:s}></{1:s}>", rootname, rootname);
                m_doc.LoadXml(s);
                m_root = m_doc.DocumentElement;
                m_current = m_root;
                m_saved = null;
            }
            else
            {

                try
                {
                    m_doc.Load(filename);
                    m_root = m_doc.DocumentElement;
                    m_current = m_root;
                    m_saved = null;

                }
                catch (System.Exception ex)
                {
                	return false;
                }
                
            }
            return true;
        }
        public bool reset()
        {
            m_doc = null;
            m_root = null;
            m_current = null;
            m_saved = null;
            return true;
        }
        public bool write_file(string filename)
        {
            XmlTextWriter tr = new XmlTextWriter(filename, null);
            tr.Formatting = Formatting.Indented;
            m_doc.WriteContentTo(tr);
            tr.Close();
            return true;
        }
        public bool back_to_root()
        {
            m_current = m_doc.DocumentElement;
            return true;
        }
        public bool back_to_parent()
        {
            XmlNode node = m_current.ParentNode;
            if (node == null)
                return false;
            if (!(node is XmlElement)) return false;
            m_current =(XmlElement) node;

            return true;
        }
        public bool save_current()
        {
            m_saved = m_current;
            return true;
        }
        public bool restore_current()
        {
            m_current = m_saved;
            return true;
        }
        /************************************************************************/
        /* find a child in given node
         * */
        /************************************************************************/
        private XmlNode find_first_node(XmlNode nodeparent, string name)
        {
            if (nodeparent == null) return null;
            XmlNode  node = nodeparent.FirstChild;
            if (node == null) return null;
            
            do 
            {
                if (node.Name == name) return node;
                node = node.NextSibling;

                
            }while (node != null);

            return null;
        }

    }
}
