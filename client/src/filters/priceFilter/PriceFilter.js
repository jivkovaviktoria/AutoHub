import {useState} from "react";
import * as CarsService from '../../services/CarService';

export const PriceFilter = ({onFilter}) => {
    const [minPrice, setMinPrice] = useState(-1);
    const [maxPrice, setMaxPrice] = useState(-1);

    const handleFilter = () => {
        CarsService.FilterByPrice({Min: minPrice, Max: maxPrice})
            .then(result => onFilter(result));
    }

    return (
        <div>
            <label>Min:</label>
            <input type='number' onInput={e => setMinPrice(e.target.value)}/>
            <label>Max:</label>
            <input type='number' onInput={e => setMaxPrice(e.target.value)}/>
            <button onClick={handleFilter}>Filter</button>
        </div>
    );
}