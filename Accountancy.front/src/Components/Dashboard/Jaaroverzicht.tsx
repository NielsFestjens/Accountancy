import React from 'react';

interface IProps {
    // jaarData?: JaarData
}

class JaarData {
    maanden!: MaandData[];
}

class MaandData {
    naam!: string;
    aantalWeekdagen!: number;
    aantalFeestdagen!: number;
    aantalVerlofdagen!: number;
    aantalWerkDagen = () => this.aantalWeekdagen - this.aantalFeestdagen - this.aantalVerlofdagen;
    
}

const Jaaroverzicht = (props: IProps) => {
    return (
        <div>
            <h3>Facturen</h3>
        </div>
    );
}

export default Jaaroverzicht;