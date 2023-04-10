import {useState} from "react";
import {PriceFilter} from "../../filters/priceFilter/PriceFilter";

export const Filter = ({onFilter}) => {
    const [isOpen, setIsOpen] = useState(false);

    const priceHandler = () => {
        setIsOpen(true);
    }

    return (
        <div>
            <button onClick={priceHandler}>Price</button>
            {isOpen && <PriceFilter onFilter={onFilter}/>}
        </div>
    );
}