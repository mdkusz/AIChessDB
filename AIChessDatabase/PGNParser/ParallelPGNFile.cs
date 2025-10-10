using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static AIChessDatabase.Properties.Resources;
using static AIChessDatabase.Properties.UIResources;

namespace AIChessDatabase.PGNParser
{
    /// <summary>
    /// PGNFile parser for parallel processing of matches.
    /// </summary>
    public class ParallelPGNFile
    {
        private List<string> _matches = new List<string>();
        private PGNMatch[] _pgnmatches = null;
        private string _filename = "";

        public ParallelPGNFile()
        {
        }
        /// <summary>
        /// Count of matches contained in the current PGN file.
        /// </summary>
        public int MatchCount
        {
            get
            {
                return _matches.Count;
            }
        }
        /// <summary>
        /// Get the match at the specified index.
        /// </summary>
        /// <param name="nm">
        /// Match index to retrieve.
        /// </param>
        /// <returns>
        /// PGNMatch object at the specified index.
        /// </returns>
        public PGNMatch GetMatch(int nm)
        {
            return _pgnmatches[nm];
        }
        /// <summary>
        /// All matches contained in the current PGN file.
        /// </summary>
        public IEnumerable<PGNMatch> Matches
        {
            get
            {
                foreach (PGNMatch m in _pgnmatches)
                {
                    yield return m;
                }
            }
        }
        /// <summary>
        /// Get the PGNFile object containing all matches parsed from the file.
        /// </summary>
        public PGNFile PGNFile
        {
            get
            {
                return new PGNFile(new List<PGNMatch>(_pgnmatches))
                {
                    FileName = _filename
                };
            }
        }
        /// <summary>
        /// Parse a PGN file and extract all matches.
        /// </summary>
        /// <param name="filename">
        /// File path to the PGN file to parse.
        /// </param>
        /// <returns>
        /// Match count found in the file.
        /// </returns>
        /// <exception cref="Exception">
        /// Raises an exception if the file does not contain any matches or if there is an error during parsing.
        /// </exception>
        public int Parse(string filename)
        {
            _filename = filename;
            using (StreamReader rdr = new StreamReader(filename))
            {
                string content = rdr.ReadToEnd().Replace("\n", "'").Replace("\r", "'").Replace("\t", " ");
                rdr.Close();
                int pos = content.IndexOf(TXT_PGNSTART);
                if (pos >= 0)
                {
                    content = content.Substring(pos);
                    SplitContent(content, TXT_PGNSTART, _matches);
                    PGNMatch[] tmpmatches = new PGNMatch[_matches.Count];
                    string error = "";
                    try
                    {
                        Parallel.For(0, _matches.Count, (m) =>
                        {
                            try
                            {
                                PGNFile pf = new PGNFile();
                                pf.ParseString(_matches[m]);
                                tmpmatches[m] = pf.GetPGNMatch(0);
                            }
                            catch (Exception ex)
                            {
                                error = ex.Message;
                                throw;
                            }
                        });
                    }
                    catch (Exception exx)
                    {
                        throw new Exception(error ?? exx.Message);
                    }
                    List<PGNMatch> pgnml = new List<PGNMatch>();
                    for (int ix = 0; ix < tmpmatches.Length; ix++)
                    {
                        if (tmpmatches[ix] != null)
                        {
                            pgnml.Add(tmpmatches[ix]);
                        }
                    }
                    _pgnmatches = pgnml.ToArray();
                }
                else
                {
                    throw new Exception(ERR_NOEVENLABEL);
                }
                return _matches.Count;
            }
        }
        /// <summary>
        /// Parse a PGN file and extract all matches, writing errors to a specified file.
        /// </summary>
        /// <param name="filename">
        /// PGN file path to parse.
        /// </param>
        /// <param name="efile">
        /// Error file path to write parsing errors.
        /// </param>
        /// <returns>
        /// Number of matches found in the file.
        /// </returns>
        public int Parse(string filename, string efile)
        {
            StreamWriter wre = null;
            _filename = filename;
            string[] errors = null;
            using (StreamReader rdr = new StreamReader(filename))
            {
                try
                {
                    string errcontent = rdr.ReadToEnd();
                    string content = errcontent.Replace("\n", "'").Replace("\r", "'").Replace("\t", " ");
                    rdr.Close();
                    int pos = content.IndexOf(TXT_PGNSTART);
                    if (pos >= 0)
                    {
                        content = content.Substring(pos);
                        SplitContent(content, TXT_PGNSTART, _matches);
                        int poserr = errcontent.IndexOf(TXT_PGNSTART);
                        errcontent = errcontent.Substring(poserr);
                        List<string> lerr = new List<string>();
                        SplitContent(errcontent, TXT_PGNSTART, lerr);

                        PGNMatch[] tmpmatches = new PGNMatch[_matches.Count];
                        errors = new string[_matches.Count];
                        Parallel.For(0, _matches.Count, (m) =>
                        {
                            try
                            {
                                PGNFile pf = new PGNFile();
                                pf.ParseString(_matches[m]);
                                tmpmatches[m] = pf.GetPGNMatch(0);
                            }
                            catch (Exception ex)
                            {
                                errors[m] = "[FileName \"" + filename + "\"]\n[Error \"" + ex.Message.Replace("\n", "").Replace("\r", "") + "\"]\n" + lerr[m];
                            }
                        });
                        List<PGNMatch> pgnml = new List<PGNMatch>();
                        for (int ix = 0; ix < tmpmatches.Length; ix++)
                        {
                            if (tmpmatches[ix] != null)
                            {
                                pgnml.Add(tmpmatches[ix]);
                            }
                        }
                        _pgnmatches = pgnml.ToArray();
                    }
                    else
                    {
                        wre = new StreamWriter(efile, true);
                        wre.WriteLine(errcontent);
                    }
                }
                finally
                {
                    if (errors != null)
                    {
                        for (int ix = 0; ix < errors.Length; ix++)
                        {
                            if (errors[ix] != null)
                            {
                                if (wre == null)
                                {
                                    wre = new StreamWriter(efile, true);
                                }
                                wre.WriteLine(errors[ix]);
                            }
                        }
                    }
                    if (wre != null)
                    {
                        wre.Close();
                    }
                }
            }
            return _matches?.Count ?? 0;
        }
        /// <summary>
        /// Split the content of a PGN file into matches based on a specified delimiter.
        /// </summary>
        /// <param name="content">
        /// Content of the PGN file to split.
        /// </param>
        /// <param name="sp">
        /// String delimiter used to split the content into matches.
        /// </param>
        /// <param name="lcontent">
        /// List to store the split matches.
        /// </param>
        private void SplitContent(string content, string sp, List<string> lcontent)
        {
            int pos = 0;
            lcontent.Clear();
            while (content.IndexOf(sp) == 0)
            {
                string substr = content.Substring(1);
                pos = substr.IndexOf(sp);
                if (pos < 0)
                {
                    lcontent.Add(content);
                    break;
                }
                else
                {
                    lcontent.Add(content.Substring(0, pos));
                    content = content.Substring(pos + 1);
                }
            }
        }
    }
}
