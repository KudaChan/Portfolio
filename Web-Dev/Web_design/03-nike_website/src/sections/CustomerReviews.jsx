import ReviewCard from '../components/ReviewCard';
import { reviews } from '../constants';

const CustomerReviews = () => {
  return (
    <section className='max-container'>
      <h3 className="font-palanquin text-center text-4xl font-bold">
        What Our
        <span className='text-coral-red'> Customers </span>
        Say?
      </h3>
      <p className="info-text m-auto mt-4 text-center max-w-lg">
        Hear genuine feedback from our customers who have experienced our products and services.
      </p>
      <div className="mt-24 flex flex-1 justify-evenly items-center max-lg:flex-col gap-14">
        {reviews.map((review, index) => (
          <ReviewCard
            key={index}
            {...review}
          />
        ))}
      </div>
    </section>
  )
}

export default CustomerReviews