using System;
using System.Collections.Generic;
using System.Text;

namespace KB9Utility
{
    public class Tokenizer
    {
        
	    private List<string> m_stra = new List<string>();
                
        /* ==========================================================================
	        Class :			CTokenizer

	        Date :			06/18/04

	        Purpose :		"CTokenizer" is a very simple class to tokenize a string 
					        into a string array.	

	        Description :	The string is chopped up and put into an internal 
					        "CStringArray". With "GetAt", different types of data can 
					        be read from the different elements.

	        Usage :			Create an instance of "CTokenizer" on the stack. Set 
					        the string to parse in the "ctor", and the delimiter if 
					        not a comma. The "ctor" automatically tokenize the input
					        string.

					        Call "GetSize" to get the number of 
					        tokens, and "GetAt" to get a specific token.
        	
					        You can also reuse the tokenizer by calling "Init".
           ========================================================================
	        Changes :		28/8  2004	Changed a char to TCHAR to allow UNICODE 
								        building (Enrico Detoma)
           ========================================================================*/



        ////////////////////////////////////////////////////////////////////
        // Public functions
        //
        Tokenizer( string strInput, string strDelimiter )
        /* ============================================================
	        Function :		CTokenizer::CTokenizer
	        Description :	Constructor.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	CString strInput				-	String to 
														        tokenize
					        const CString & strDelimiter	-	Delimiter, 
														        defaults to
														        comma.
	        Usage :			Should normally be created on the stack.

           ============================================================*/
        {

	        Init(strInput, strDelimiter);

        }

        void Init( string strInput, string strDelimiter )
        /* ============================================================
	        Function :		CTokenizer::Init
	        Description :	Reinitializes and tokenizes the tokenizer 
					        with "strInput". "strDelimiter" is the 
					        delimiter to use.
	        Access :		Public
        					
	        Return :		void
	        Parameters :	const CString & strInput		-	New string
					        const CString & strDelimiter	-	Delimiter,
														        defaults to 
														        comma

	        Usage :			Call to reinitialize the tokenizer.

           ============================================================*/
        {

	        //CString copy( strInput );
            string copy = strInput;
	        m_stra.Clear();
	        int nFound = copy.IndexOf( strDelimiter );

	        while(nFound != -1)
	        {
		        string strLeft;
		        strLeft = copy.Substring(0, nFound );
		        copy = copy.Substring(nFound+1 );

		        m_stra.Add( strLeft );
		        nFound = copy.IndexOf( strDelimiter );
	        }

	        m_stra.Add( copy );

        }

        public int GetSize(  ) 
        /* ============================================================
	        Function :		CTokenizer::GetSize
	        Description :	Gets the number of tokens in the tokenizer.
	        Access :		Public
        					
	        Return :		int	-	Number of tokens.
	        Parameters :	none

	        Usage :			Call to get the number of tokens in the 
					        tokenizer.

           ============================================================*/
        {

	        return m_stra.Count;

        }

        public void GetAt( int nIndex, ref string str ) 
        /* ============================================================
	        Function :		CTokenizer::GetAt
	        Description :	Get the token at "nIndex" and put it in 
					        "str".
	        Access :		Public
        					
	        Return :		void
	        Parameters :	int nIndex		- Index to get token from
					        CString & str	- Result

	        Usage :			Call to get the value of the token at 
					        "index".

           ============================================================*/
        {

		        if( nIndex < m_stra.Count )
			        str = m_stra[ nIndex ];
		        else
			        str = "";

        }

        public void GetAt( int nIndex, ref int var )
        /* ============================================================
	        Function :		CTokenizer::GetAt
	        Description :	Get the token at "nIndex" and put it in 
					        "var".
	        Access :		Public
        					
	        Return :		void
	        Parameters :	int nIndex	- Index to get token from
					        int & var	- Result

	        Usage :			Call to get the value of the token at 
					        "index".

           ============================================================*/
        {

		        if( nIndex < m_stra.Count )
			        var = int.Parse( m_stra[ nIndex ] );
		        else
			        var = 0;

        }

        public void GetAt( int nIndex, ref long  var ) 
        /* ============================================================
	        Function :		CTokenizer::GetAt
	        Description :	Get the token at "nIndex" and put it in 
					        "var".
	        Access :		Public
        					
	        Return :		void
	        Parameters :	int nIndex	- Index to get token from
					        WORD & var	- Result

	        Usage :			Call to get the value of the token at 
					        "index".

           ============================================================*/
        {

		        if( nIndex < m_stra.Count )
			        var = long.Parse(m_stra[nIndex ] );
		        else
			        var = 0;

        }

        public void GetAt( int nIndex, ref double var ) 
        /* ============================================================
	        Function :		CTokenizer::GetAt
	        Description :	Get the token at "nIndex" and put it in 
					        "var".
	        Access :		Public
        					
	        Return :		void
	        Parameters :	int nIndex		- Index to get token from
					        double & var	- Result

	        Usage :			Call to get the value of the token at 
					        "index".

           ============================================================*/
        {

		     
		        if( nIndex < m_stra.Count )
			        var = double.Parse( m_stra[ nIndex ] );
		        else
			        var = 0.0;

        }

        //public void GetAt( int nIndex, ref long  var ) 
        ///* ============================================================
        //    Function :		CTokenizer::GetAt
        //    Description :	Get the token at "nIndex" and put it in 
        //                    "var".
        //    Access :		Public
        					
        //    Return :		void
        //    Parameters :	int nIndex	- Index to get token from
        //                    DWORD & var	- Result

        //    Usage :			Call to get the value of the token at 
        //                    "index".

        //   ============================================================*/
        //{

        //        if( nIndex < m_stra.GetSize() )
        //            var = static_cast< DWORD >( _ttoi( m_stra.GetAt( nIndex ) ) );
        //        else
        //            var = 0;

        //}


    }
}
