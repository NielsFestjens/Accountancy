import ApiCaller from 'Infrastructure/ApiCaller';
import { apiUri } from 'config';
import { InvoiceDto } from './models';
import { InvoiceStatus } from 'Components/Invoices/models';

const apiCaller = new ApiCaller(apiUri + 'Dashboard/');

export const getDashboardInvoices = (onError: (message: string) => void,): Promise<InvoiceDto[]> => apiCaller.get(onError, 'getInvoices').then(response => response.content);
export const updateInvoiceStatus = (onError: (message: string) => void, invoice: InvoiceDto, status: InvoiceStatus): Promise<any> => apiCaller.post(onError, 'updateInvoiceStatus', { id: invoice.id, status });

export const exportPeppol = (onError: (message: string) => void, invoiceId: number, file: File): Promise<{blob: Blob, filename: string}> => {
    const formData = new FormData();
    formData.append('file', file);
    
    return fetch(`${apiUri}Invoices/ExportPeppol/${invoiceId}`, {
        method: 'POST',
        credentials: 'include',
        body: formData
    }).then(async response => {
        if (!response.ok) {
            onError('Er is een fout opgetreden bij het exporteren van de Peppol XML');
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        // Extract filename from Content-Disposition header
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = 'peppol.xml';
        if (contentDisposition) {
            const matches = contentDisposition.match(/filename=\"?([^\"]+)\"?/);
            if (matches && matches[1]) {
                filename = matches[1];
            }
        }
        
        const blob = await response.blob();
        return { blob, filename };
    }).catch(error => {
        onError(error.message);
        console.error(error);
        throw error;
    });
};

export const combinePdf = (onError: (message: string) => void, invoiceId: number, file: File): Promise<{blob: Blob, filename: string}> => {
    const formData = new FormData();
    formData.append('file', file);
    
    return fetch(`${apiUri}Invoices/CombinePdf/${invoiceId}`, {
        method: 'POST',
        credentials: 'include',
        body: formData
    }).then(async response => {
        if (!response.ok) {
            onError('Er is een fout opgetreden bij het combineren van de PDF');
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        // Extract filename from Content-Disposition header
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = 'combined.pdf';
        debugger;
        if (contentDisposition) {
            const matches = contentDisposition.match(/filename=\"?([^\"]+)\"?/);
            if (matches && matches[1]) {
                filename = matches[1];
            }
        }
        
        const blob = await response.blob();
        return { blob, filename };
    }).catch(error => {
        onError(error.message);
        console.error(error);
        throw error;
    });
};