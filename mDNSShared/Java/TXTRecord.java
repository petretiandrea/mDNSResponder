/*
 * Copyright (c) 2004 Apple Computer, Inc. All rights reserved.
 *
 * @APPLE_LICENSE_HEADER_START@
 * 
 * This file contains Original Code and/or Modifications of Original Code
 * as defined in and that are subject to the Apple Public Source License
 * Version 2.0 (the 'License'). You may not use this file except in
 * compliance with the License. Please obtain a copy of the License at
 * http://www.opensource.apple.com/apsl/ and read it before using this
 * file.
 * 
 * The Original Code and all software distributed under the License are
 * distributed on an 'AS IS' basis, WITHOUT WARRANTY OF ANY KIND, EITHER
 * EXPRESS OR IMPLIED, AND APPLE HEREBY DISCLAIMS ALL SUCH WARRANTIES,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT OR NON-INFRINGEMENT.
 * Please see the License for the specific language governing rights and
 * limitations under the License.
 * 
 * @APPLE_LICENSE_HEADER_END@

    Change History (most recent first):

$Log: TXTRecord.java,v $
Revision 1.5  2004/08/25 21:54:36  rpantos
<rdar://problem/3773973> Fix getValue() for values containing '='.

Revision 1.4  2004/08/04 01:04:50  rpantos
<rdar://problems/3731579&3731582> Fix set(); add remove() & toString().

Revision 1.3  2004/07/13 21:24:25  rpantos
Fix for <rdar://problem/3701120>.

Revision 1.2  2004/04/30 21:48:27  rpantos
Change line endings for CVS.

Revision 1.1  2004/04/30 16:29:35  rpantos
First checked in.

	To do:
	- implement remove()
	- fix set() to replace existing values
 */


package	com.apple.dnssd;


/**	
	Object used to construct and parse DNS-SD format TXT records.
	For more info see <a href="http://files.dns-sd.org/draft-cheshire-dnsext-dns-sd.txt">DNS-Based Service Discovery</a>, section 6. 
*/

public class	TXTRecord
{
	/* 
	DNS-SD specifies that a TXT record corresponding to an SRV record consist of
	a packed array of bytes, each preceded by a length byte. Each string
	is an attribute-value pair. 
	
	The TXTRecord object stores the entire TXT data as a single byte array, traversing it
	as need be to implement its various methods.
	*/

	static final protected byte		kAttrSep = '=';

	protected byte[]		fBytes;

	/** Constructs a new, empty TXT record. */
	public		TXTRecord() 
	{ fBytes = new byte[0]; }

	/** Constructs a new TXT record from a byte array in the standard format. */
	public		TXTRecord( byte[] initBytes) 
	{ fBytes = (byte[]) initBytes.clone(); }

	/** Set a key/value pair in the TXT record. Setting an existing key will replace its value.<P>
		@param	key
					The key name. Must be ASCII, with no '=' characters.
		<P>
		@param	value
					Value to be encoded into bytes using the default platform character set.
	*/
	public void	set( String key, String value)
	{
		byte[]	valBytes = (value != null) ? value.getBytes() : null;
		this.set( key, valBytes);
	}

	/** Set a key/value pair in the TXT record. Setting an existing key will replace its value.<P>
		@param	key
					The key name. Must be ASCII, with no '=' characters.
		<P>
		@param	value
					Binary representation of the value.
	*/
	public void	set( String key, byte[] value)
	{
		byte[]	keyBytes;
		int		valLen = (value != null) ? value.length : 0;

		try {
			keyBytes = key.getBytes( "US-ASCII");
		}
		catch ( java.io.UnsupportedEncodingException uee) {
			throw new IllegalArgumentException();
		}

		for ( int i=0; i < keyBytes.length; i++)
			if ( keyBytes[i] == '=')
				throw new IllegalArgumentException();

		if ( keyBytes.length + valLen >= 255)
			throw new ArrayIndexOutOfBoundsException();

		int		prevLoc = this.remove( key);
		if ( prevLoc == -1)
			prevLoc = this.size();

		this.insert( keyBytes, value, prevLoc);
	}

	protected void	insert( byte[] keyBytes, byte[] value, int index)
	// Insert a key-value pair at index
	{
		byte[]	oldBytes = fBytes;
		int		valLen = (value != null) ? value.length : 0;
		int		insertion = 0;
		byte	newLen, avLen;
	
		// locate the insertion point
		for ( int i=0; i < index && insertion < fBytes.length; i++)
			insertion += fBytes[ insertion] + 1;
	
		avLen = (byte) ( keyBytes.length + valLen + (value != null ? 1 : 0));
		newLen = (byte) ( avLen + oldBytes.length + 1);

		fBytes = new byte[ newLen];
		System.arraycopy( oldBytes, 0, fBytes, 0, insertion);
		int secondHalfLen = oldBytes.length - insertion;
		System.arraycopy( oldBytes, insertion, fBytes, newLen - secondHalfLen, secondHalfLen);
		fBytes[ insertion] = avLen;
		System.arraycopy( keyBytes, 0, fBytes, insertion + 1, keyBytes.length);
		if ( value != null)
		{
			fBytes[ insertion + 1 + keyBytes.length] = kAttrSep;
			System.arraycopy( value, 0, fBytes, insertion + keyBytes.length + 2, valLen);
		}
	}

	/** Remove a key/value pair from the TXT record. Returns index it was at, or -1 if not found. */
	public int	remove( String key)
	{
		int		avStart = 0;

		for ( int i=0; avStart < fBytes.length; i++)
		{
			int		avLen = fBytes[ avStart];
			if ( key.length() <= avLen && 
				 ( key.length() == avLen || fBytes[ avStart + key.length() + 1] == kAttrSep))
			{
				String	s = new String( fBytes, avStart + 1, key.length());
				if ( 0 == key.compareToIgnoreCase( s))
				{
					byte[]	oldBytes = fBytes;
					fBytes = new byte[ oldBytes.length - avLen - 1];
					System.arraycopy( oldBytes, 0, fBytes, 0, avStart);
					System.arraycopy( oldBytes, avStart + avLen + 1, fBytes, avStart, oldBytes.length - avStart - avLen - 1);
					return i;
				}
			}
			avStart += avLen + 1;
		}
		return -1;
	}

	/**	Return the number of keys in the TXT record. */
	public int	size()
	{
		int		i, avStart;

		for ( i=0, avStart=0; avStart < fBytes.length; i++)
			avStart += fBytes[ avStart] + 1;
		return i;
	}

	/** Return true if key is present in the TXT record, false if not. */
	public boolean	contains( String key)
	{		
		String	s = null;

		for ( int i=0; null != ( s = this.getKey( i)); i++)
			if ( 0 == key.compareToIgnoreCase( s))
				return true;
		return false;
	}

	/**	Return a key in the TXT record by zero-based index. Returns null if index exceeds the total number of keys. */
	public String	getKey( int index)
	{
		int		avStart = 0;

		for ( int i=0; i < index && avStart < fBytes.length; i++)
			avStart += fBytes[ avStart] + 1;

		if ( avStart < fBytes.length)
		{
			int	avLen = fBytes[ avStart];
			int	aLen = 0;
			
			for ( aLen=0; aLen < avLen; aLen++)
				if ( fBytes[ avStart + aLen + 1] == kAttrSep)
					break;
			return new String( fBytes, avStart + 1, aLen);
		}
		return null;
	}

	/**	
		Look up a key in the TXT record by zero-based index and return its value. <P>
		Returns null if index exceeds the total number of keys. 
		Returns null if the key is present with no value.
	*/
	public byte[]	getValue( int index)
	{
		int		avStart = 0;
		byte[]	value = null;

		for ( int i=0; i < index && avStart < fBytes.length; i++)
			avStart += fBytes[ avStart] + 1;

		if ( avStart < fBytes.length)
		{
			int	avLen = fBytes[ avStart];
			int	aLen = 0;
			
			for ( aLen=0; aLen < avLen; aLen++)
			{
				if ( fBytes[ avStart + aLen + 1] == kAttrSep)
				{
					value = new byte[ avLen - aLen - 1];
					System.arraycopy( fBytes, avStart + aLen + 2, value, 0, avLen - aLen - 1);
					break;
				}
			}
		}
		return value;
	}

	/** Converts the result of getValue() to a string in the platform default character set. */
	public String	getValueAsString( int index)
	{
		byte[]	value = this.getValue( index);
		return value != null ? new String( value) : null;
	}

	/**	Get the value associated with a key. Will be null if the key is not defined.
		Array will have length 0 if the key is defined with an = but no value.<P> 

		@param	forKey
					The left-hand side of the key-value pair.
		<P>
		@return		The binary representation of the value.
	*/
	public byte[]	getValue( String forKey)
	{
		String	s = null;
		int		i;

		for ( i=0; null != ( s = this.getKey( i)); i++)
			if ( 0 == forKey.compareToIgnoreCase( s))
				return this.getValue( i);
		return null;
	}

	/**	Converts the result of getValue() to a string in the platform default character set.<P> 

		@param	forKey
					The left-hand side of the key-value pair.
		<P>
		@return		The value represented in the default platform character set.
	*/
	public String	getValueAsString( String forKey)
	{
		byte[]	val = this.getValue( forKey);
		return val != null ? new String( val) : null;
	}

	/** Return the contents of the TXT record as raw bytes. */
	public byte[]	getRawBytes() { return (byte[]) fBytes.clone(); }

	/** Return a string representation of the object. */
	public String	toString()
	{
		String		a, result = null;
		
		for ( int i=0; null != ( a = this.getKey( i)); i++)
		{
			String av = String.valueOf( i) + "={" + a;
			String val = this.getValueAsString( i);
			if ( val != null)
				av += "=" + val + "}";
			else
				av += "}";
			if ( result == null)
				result = av;
			else
				result = result + ", " + av;
		}
		return result != null ? result : "";
	}
}

