export default function formatFullAddress(lesseeContact, addlineBreakInd) {
  return `${lesseeContact.address1 || ''} ${lesseeContact.address2 || ''}
                                           ${addlineBreakInd ? '</br>' : ''}
                                          ${lesseeContact.city || ''}
                                          ${lesseeContact.state || ''},
                                          ${lesseeContact.zip || ''}`;
}
