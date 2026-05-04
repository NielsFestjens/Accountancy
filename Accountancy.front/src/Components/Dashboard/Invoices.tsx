import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import {  InvoiceDto, InvoiceYear } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';
import { apiUri } from 'config';
import { combinePdf, exportPeppol } from './DataService';

interface IProps {
    invoices: InvoiceYear[];
    updateInvoiceStatus: (invoice: InvoiceDto, status: InvoiceStatus) => void;
}

const Invoices = (props: IProps) => {

    const { invoices, updateInvoiceStatus } = props;
    const [isCombing, setIsCombining] = useState<{[key: number]: boolean}>({});
    const [isExporting, setIsExporting] = useState<{[key: number]: boolean}>({});

    const handleCombinePdfClick = (invoiceId: number) => {
        // Create a hidden file input and trigger it
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.accept = '.pdf';
        fileInput.style.display = 'none';
        
        fileInput.onchange = (event) => {
            const target = event.target as HTMLInputElement;
            const file = target.files?.[0];
            if (file) {
                handleCombinePdfWithFile(invoiceId, file);
            }
            document.body.removeChild(fileInput);
        };
        
        document.body.appendChild(fileInput);
        fileInput.click();
    };

    const handleExportPeppolClick = (invoiceId: number) => {
        // Create a hidden file input and trigger it
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.accept = '.pdf';
        fileInput.style.display = 'none';
        
        fileInput.onchange = (event) => {
            const target = event.target as HTMLInputElement;
            const file = target.files?.[0];
            if (file) {
                handleExportPeppolWithFile(invoiceId, file);
            }
            document.body.removeChild(fileInput);
        };
        
        document.body.appendChild(fileInput);
        fileInput.click();
    };

    const handleExportPeppolWithFile = async (invoiceId: number, file: File) => {
        setIsExporting(prev => ({ ...prev, [invoiceId]: true }));
        
        try {
            const result = await exportPeppol(
                (error) => alert(`Fout: ${error}`),
                invoiceId,
                file
            );
            
            // Create download link
            const url = window.URL.createObjectURL(result.blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = result.filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
            
        } catch (error) {
            console.error('Error exporting Peppol XML:', error);
        } finally {
            setIsExporting(prev => ({ ...prev, [invoiceId]: false }));
        }
    };

    const handleCombinePdfWithFile = async (invoiceId: number, file: File) => {
        setIsCombining(prev => ({ ...prev, [invoiceId]: true }));
        
        try {
            const result = await combinePdf(
                (error) => alert(`Fout: ${error}`),
                invoiceId,
                file
            );
            
            // Create download link
            const url = window.URL.createObjectURL(result.blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = result.filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
            
        } catch (error) {
            console.error('Error combining PDF:', error);
        } finally {
            setIsCombining(prev => ({ ...prev, [invoiceId]: false }));
        }
    };

    return (
        <div>
            <h3>Facturen</h3>
            <table className="table">
                <thead>
                    <tr>
                        <th style={{width: "70px"}}>Jaar</th>
                        <th style={{width: "70px"}}>Maand</th>
                        <th>Bestemmeling</th>
                        <th>Bedrag</th>
                        <th>Status</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    { invoices.map(invoiceYear =>
                        <React.Fragment key={`year_${invoiceYear.year}`}>
                            <tr>
                                <td><b>{invoiceYear.year}</b></td>
                                <td></td>
                                <td><b>Totaal</b></td>
                                <td><b>{invoiceYear.months.map(x => x.invoices.map(i => i.total).reduce((a, b) => a + b, 0)).reduce((a, b) => a + b, 0).toFixed(2)}</b></td>
                            </tr>
                            { invoiceYear.months.map(invoiceMonth =>
                                <React.Fragment key={`month_${invoiceYear.year}_${invoiceMonth.month}`}>
                                    <tr>
                                        <td></td>
                                        <td><b>{invoiceMonth.month}</b></td>
                                        <td><b>Totaal</b></td>
                                        <td><b>{invoiceMonth.invoices.map(x => x.total).reduce((a, b) => a + b, 0).toFixed(2)}</b></td>
                                    </tr>
                                    {invoiceMonth.invoices.map(x => 
                                        <tr key={x.id}>
                                            <td></td>
                                            <td></td>
                                            <td><Link to={`/Invoices/Invoice/${x.id}`}>{x.receivingCompany}</Link></td>
                                            <td>{ x.total.toFixed(2) }</td>
                                            <td>{ InvoiceStatus[x.status] }</td>
                                            <td>
                                                <div style={{display: 'flex', alignItems: 'center', gap: '10px', flexWrap: 'wrap'}}>
                                                    <a href={`${apiUri}Invoices/PrintPdf?id=${x.id}`} title="Bekijk pdf" target="_blank" rel="noreferrer"><i className="fa fa-file-pdf-o action-icon"></i></a>
                                                    
                                                    <button 
                                                        className="link" 
                                                        onClick={() => handleCombinePdfClick(x.id)}
                                                        disabled={isCombing[x.id]}
                                                        title="Combineer PDF met timesheet"
                                                    >
                                                        {isCombing[x.id] ? <i className="fa fa-spinner fa-spin action-icon"></i> : <i className="fa fa-paperclip action-icon"></i>}
                                                    </button>

                                                    <button 
                                                        className="link" 
                                                        onClick={() => handleExportPeppolClick(x.id)}
                                                        disabled={isExporting[x.id]}
                                                        title="Exporteer UBL XML"
                                                    >
                                                        {isExporting[x.id] ? <i className="fa fa-spinner fa-spin action-icon"></i> : <i className="fa fa-file-code-o action-icon"></i>}
                                                    </button>

                                                    { x.status === InvoiceStatus.Sent && 
                                                        <button className="link" onClick={() => updateInvoiceStatus(x, InvoiceStatus.Paid)}><i className="fa fa-credit-card action-icon" title="Markeer als betaald"></i></button>
                                                    }
                                                </div>
                                            </td>
                                        </tr>
                                    )}
                                </React.Fragment>
                            )}
                        </React.Fragment>
                    )}
            </tbody>
        </table>
        </div>
    );
}

export default Invoices;