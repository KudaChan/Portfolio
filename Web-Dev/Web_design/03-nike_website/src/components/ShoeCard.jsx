import PropTypes from 'prop-types';

const ShoeCard = ({ imgSrc, changeBigShoeImage, bigShoeImage }) => {
    const handleClick = () => {
        if (bigShoeImage !== imgSrc.bigShoe) {
            changeBigShoeImage(imgSrc.bigShoe);
        }
    };

    return (
        <div
            className={`border-2 rounded-xl
                
                ${bigShoeImage === imgSrc.bigShoe ? 'border-coral-red' : 'border-transparent'}
                cursor-pointer max-sm:flex-1
            `}
            onClick={handleClick}
        >
            <div className='flex justify-center items-center bg-card bg-center bg-cover sm:w-40 sm:h-40 rounded-xl max-sm:p-4'>
                <img
                    src={imgSrc.thumbnail}
                    alt='shoe'
                    width={150}
                    height={150}
                    className='object-contain'
                />
            </div>
        </div>
    )
}

ShoeCard.propTypes = {
    imgSrc: PropTypes.object.isRequired,
    changeBigShoeImage: PropTypes.func.isRequired,
    bigShoeImage: PropTypes.string.isRequired,
};

export default ShoeCard;