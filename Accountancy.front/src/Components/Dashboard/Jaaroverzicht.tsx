import { Fragment, useState } from 'react';
import { groupBy } from 'Infrastructure/GroupBy';

interface IProps {
    // jaarData?: JaarData
}

class MaandData {
    jaar!: number;
    maand!: Maand;
    aantalWeekdagen!: number;
    aantalFeestdagen!: number;
    aantalVerlofdagen!: number;
    uren!: number;
    dagPrijs!: number;
}

const aantalWerkDagen = (maand: MaandData) => maand.aantalWeekdagen - maand.aantalFeestdagen - maand.aantalVerlofdagen;
const bedrag = (maand: MaandData) => aantalWerkDagen(maand) * maand.dagPrijs;

enum Maand {
    Januari = 1,
    Februari,
    Maart,
    April,
    Mei,
    Juni,
    Juli,
    Augustus,
    September,
    Oktober,
    November,
    December
}

const initialMaandData: MaandData[] = [
    { jaar: 2021, maand:  1 as Maand, aantalWeekdagen: 21, aantalFeestdagen: 1, aantalVerlofdagen:  0.0, uren: 160.00, dagPrijs: 540 },
    { jaar: 2021, maand:  2 as Maand, aantalWeekdagen: 20, aantalFeestdagen: 0, aantalVerlofdagen:  0.0, uren: 158.00, dagPrijs: 540 },
    { jaar: 2021, maand:  3 as Maand, aantalWeekdagen: 23, aantalFeestdagen: 0, aantalVerlofdagen:  0.0, uren: 186.00, dagPrijs: 540 },
    { jaar: 2021, maand:  4 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 1, aantalVerlofdagen:  0.0, uren: 168.00, dagPrijs: 540 },
    { jaar: 2021, maand:  5 as Maand, aantalWeekdagen: 21, aantalFeestdagen: 2, aantalVerlofdagen:  0.0, uren: 153.00, dagPrijs: 540 },
    { jaar: 2021, maand:  6 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 0, aantalVerlofdagen:  1.0, uren: 172.25, dagPrijs: 540 },
    { jaar: 2021, maand:  7 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 1, aantalVerlofdagen: 12.0, uren:  75.33, dagPrijs: 540 },
    { jaar: 2021, maand:  8 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 0, aantalVerlofdagen: 12.0, uren:  80.83, dagPrijs: 540 },
    { jaar: 2021, maand:  9 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 0, aantalVerlofdagen:  3.0, uren: 157.24, dagPrijs: 560 },
    { jaar: 2021, maand: 10 as Maand, aantalWeekdagen: 21, aantalFeestdagen: 0, aantalVerlofdagen:  0.0, uren: 168.00, dagPrijs: 560 },
    { jaar: 2021, maand: 11 as Maand, aantalWeekdagen: 22, aantalFeestdagen: 2, aantalVerlofdagen:  0.0, uren: 160.00, dagPrijs: 560 },
    { jaar: 2021, maand: 12 as Maand, aantalWeekdagen: 23, aantalFeestdagen: 0, aantalVerlofdagen: 10.0, uren: 104.00, dagPrijs: 560 },
];

const Jaaroverzicht = (props: IProps) => {

    const [maandData, setMaandData] = useState<MaandData[]>(initialMaandData);
    const jaarData = groupBy(maandData, x => x.jaar).map(x => ({ jaar: x.key, maanden: x.items }));

    return (
        <div>
            <h3>Jaar overzicht</h3>
            {jaarData.map(jaarData => <Fragment key={jaarData.jaar}>
                Jaar: {jaarData.jaar}<br />
                <table className="table">
                    <thead>
                        <tr>
                            <th>Maand</th>
                            <th>Weekd.</th>
                            <th>Feestd.</th>
                            <th>Verlofd.</th>
                            <th>Werkd.</th>
                            <th>uren</th>
                        </tr>
                    </thead>
                    <tbody>
                        {jaarData.maanden.map(maandData => <tr key={maandData.maand}>
                            <td>{maandData.maand}</td>
                            <td>{maandData.aantalWeekdagen}</td>
                            <td>{maandData.aantalFeestdagen}</td>
                            <td>{maandData.aantalVerlofdagen}</td>
                            <td>{aantalWerkDagen(maandData)}</td>
                            <td>{maandData.uren}</td>
                            <td>{maandData.dagPrijs}</td>
                            <td>{bedrag(maandData)}</td>
                        </tr>)}
                    </tbody>
                </table>
            </Fragment>)}
        </div>
    );
}

export default Jaaroverzicht;